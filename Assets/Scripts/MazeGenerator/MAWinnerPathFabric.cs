using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAPathDescriptionPoint {
    public int stoneIndex;
    public MAPathDescriptionTurn direction;

    public MAPathDescriptionPoint(int stoneIndex, MAPathDescriptionTurn direction) {
        this.stoneIndex = stoneIndex;
        this.direction = direction;
    }
}


public class MAWinnerPathFabric : MAPathFabric {


    public MAPathFabric standardPathFabric;

    public GameObject borderCircleStone;

    public List<MAPathDescriptionPoint> pathDescription = new List<MAPathDescriptionPoint>();

    public int predictionFrames;

    private MAPathFabric predictor;


    public int maxIterations;
    private int iterationCounter = 0;

    private bool predictorMoved = false;


    private bool shortenInNextFrame = false;

    private int framesSinceFirstUpdate = 0;
    private bool isPredictorStuck = false;

    public int framesTillUpdate;

    private bool crashed = false;


    private int triesToFreePredictor = 0;

    public int maxTriesToFreePredictor;

    private bool showPrediction = true;

    [Range(0, 200)]
    public int labyrinthRadius;
    public bool won = false;
    public GameObject borderCircleStonesContainer;

    private int deadEndDescriptionIndex = 0;

    [Range(0, 50)]
    public int framesTillNextDeadEndGenerator;
    private int framesSinceDeadEnderFirstIteration = 0;


    void Start() {
        this.GenerateBorderCircle();
        this.InstantiatePrognoseFabric();
    }

    private void GenerateBorderCircle() {
        float u = (float)(2 * Math.PI * this.labyrinthRadius);
        float r = this.labyrinthRadius;


        int amountStones = (int)u;

        for (int stoneIndex = 0; stoneIndex < amountStones; stoneIndex++) {
            float phi = ((float)stoneIndex / (float)amountStones * 360);

            Vector3 position = new Vector3(r * Mathf.Cos(phi), 0, r * Mathf.Sin(phi));
            Vector3 rotation = new Vector3(0, Mathf.Rad2Deg * -phi);

            GameObject newBorderStone = GameObject.Instantiate(this.borderCircleStone, position, Quaternion.Euler(rotation), this.borderCircleStonesContainer.transform);
            newBorderStone.SetActive(true);
        }

    }

    void Update() {
        if (this.framesSinceFirstUpdate % this.framesTillUpdate != 0) {
            this.framesSinceFirstUpdate++;
            return;
        }

        if (this.won) {


            this.UpdateDeadEndGeneration();

            this.framesSinceDeadEnderFirstIteration++;
        }
        else {
            this.UpdateWinningPath();
        }
    }

    private void UpdateDeadEndGeneration() {
        if (this.framesSinceDeadEnderFirstIteration % this.framesTillNextDeadEndGenerator != 0) {
            return;
        }

        this.deadEndDescriptionIndex++;

        if (this.deadEndDescriptionIndex < this.pathDescription.Count - 2) {

            this.InstantiateDeadEnderAt(this.pathDescription.Count - 1 - this.deadEndDescriptionIndex);
        }
    }

    private void InstantiateDeadEnderAt(int index) {

        MAPathDescriptionPoint descriptionPoint = this.pathDescription[index];

        this.InstantiateGeneratorAtPoint(descriptionPoint);
    }


    public MAPathFabric InstantiateGeneratorAtPoint(MAPathDescriptionPoint descriptionPoint) {
        GameObject newGenerator = GameObject.Instantiate(new GameObject("PathGenerator"));
        GameObject newRotator = GameObject.Instantiate(new GameObject("PathRotator"), newGenerator.transform);
        GameObject newGeneratedStonesContainer = GameObject.Instantiate(new GameObject("GeneratedStonesContainer"), newGenerator.transform);

        MAPathFabric newFabric = this.GetInstantiatedNewFabric(newRotator.transform);
        newFabric.isPrognoseFabric = false;
        newFabric.reproductionChance = this.reproductionChance;
        newFabric.pathFabricRotator = newRotator;
        newFabric.generatedStonesContainer = newGeneratedStonesContainer;

        newFabric.enabled = true;

        Transform currentStone = this.predictor.generatedStonesList[descriptionPoint.stoneIndex + 2].transform;



        //going opposite direction
        newFabric.Initialize(currentStone.position, Quaternion.Euler(new Vector3(0, -180)) * currentStone.rotation);

        MAPathDescriptionTurn currentTurn = descriptionPoint.direction;
        MAPathDescriptionTurn newTurn = currentTurn + 2;
        newTurn = this.ModuloDirection(newTurn);

        newFabric.direction = (MAPathFabricDirection)newTurn;

        newFabric.ModuloDirection();



        return newFabric;
    }

    private void UpdateWinningPath() {
        if (this.LoopPrognoseUpdates()) {
            this.predictor.WritePathToWinnerFabric();

            this.DeleteTempDescriptionAndStones();


            this.triesToFreePredictor = 0;

        }
        else {
            this.triesToFreePredictor++;

            if (this.triesToFreePredictor > this.maxTriesToFreePredictor) {
                this.ShortenDescriptionAndSetPredictor(1);

                this.DeleteTempDescriptionAndStones();


                this.triesToFreePredictor = 0;
            }
        }

        this.ResetPredictorPosition();
    }

    private void DeleteTempDescriptionAndStones() {
        this.predictor.ClearTempDescriptionPoints();
        this.predictor.DestroyTempPredictionStones();
    }

    private bool LoopPrognoseUpdates() {

        for (int frameIndex = 0; frameIndex < this.predictionFrames; frameIndex++) {

            if (!this.UpdatePrognoseFabric(frameIndex)) {
                return false;
            }

            if (this.won) {
                return true;
            }
        }

        return true;
    }

    public void Win() {
        this.predictor.WritePathToWinnerFabric();
        this.won = true;
    }


    public void ResetPredictorPosition() {
        if (this.pathDescription.Count <= 0) {
            return;
        }

        MAPathDescriptionPoint descriptionPoint = this.pathDescription[this.pathDescription.Count - 1];

        this.SetPredictorToStone(descriptionPoint);
    }

    private void ShortenDescriptionAndSetPredictor(int iteration) {

        for (int iterationIndex = 0; iterationIndex < iteration; iterationIndex++) {

            int descriptionIndex = this.pathDescription.Count - 1;

            MAPathDescriptionPoint prevPoint = null;
            if (descriptionIndex > 1) {
                prevPoint = this.pathDescription[descriptionIndex - 1];

            }
            else return;

            MAPathDescriptionPoint descriptionPoint = this.pathDescription[descriptionIndex];

            this.SetPredictorToStone(descriptionPoint);

            if (prevPoint != null)
                this.TestOtherPossibleTurns(descriptionIndex, prevPoint, descriptionPoint);

            this.pathDescription.RemoveAt(this.pathDescription.Count - 1);
        }

    }



    private void TestOtherPossibleTurns(int descriptionIndex, MAPathDescriptionPoint prevPoint, MAPathDescriptionPoint descriptionPoint) {

        MAPathDescriptionTurn prevTurn = prevPoint.direction;


        MAPathDescriptionTurn currentTurn = descriptionPoint.direction;
        Debug.Log("damals war es " + currentTurn);


        //checkOppositeTurn
        MAPathDescriptionTurn newTurn = currentTurn + 2;
        Debug.Log(newTurn + "wird nach modulo ");
        newTurn = this.ModuloDirection(newTurn);
        Debug.Log("zu " + newTurn);

        this.predictor.transform.Rotate(new Vector3(0, 180));
        this.predictor.direction = (MAPathFabricDirection)newTurn;
    }



    private MAPathDescriptionTurn ModuloDirection(MAPathDescriptionTurn old) {
        MAPathFabricDirection casted = (MAPathFabricDirection)old;
        old = (MAPathDescriptionTurn)ModuloDirection(casted);
        return old;
    }



    private bool SetPredictorToStone(MAPathDescriptionPoint descriptionPoint) {
        int stoneIndex = descriptionPoint.stoneIndex;

        int stoneCount = this.predictor.generatedStonesList.Count;

        Debug.Log("stoneCount: " + stoneCount);
        Debug.Log("stoneIndex: " + stoneIndex);


        if (stoneIndex >= stoneCount) {
            return false;
        }

        Transform lastStone = this.predictor.generatedStonesList[stoneIndex].transform;


        this.predictor.Initialize(lastStone.position, Quaternion.Euler(new Vector3(0, 180)) * lastStone.rotation);

        this.predictor.DestroyGeneratedStones(stoneIndex);


        return true;
    }

    private bool UpdatePrognoseFabric(int frameIndex) {
        this.predictor.CalculateAngularVelocity();

        if (!this.predictor.Move()) {
            this.predictor.fatalCrashed = true;
            return false;
        }

        this.CheckMagnitudeAgainstRadius();

        return true;
    }

    private void CheckMagnitudeAgainstRadius() {
    }

    private void InstantiatePrognoseFabric() {
        if (this.predictor) {
            GameObject.Destroy(this.predictor.gameObject);
        }
        this.predictor = this.InstantiateNewGenerator();
        this.predictor.enabled = false;
        this.predictor.isPrognoseFabric = true;
    }

    public void AddPathDescriptionPoint(int stoneIndex, MAPathDescriptionTurn pathDescriptionTurn) {
        this.pathDescription.Add(new MAPathDescriptionPoint(stoneIndex, pathDescriptionTurn));
    }

    public override MAPathFabric GetInstantiatedNewFabric(Transform parent) {
        MAPathFabric newFabric = GameObject.Instantiate(this.standardPathFabric, this.transform.position, this.transform.rotation, parent).GetComponentInChildren<MAPathFabric>();
        newFabric.winnerPathFabric = this;
        return newFabric;
    }
}
