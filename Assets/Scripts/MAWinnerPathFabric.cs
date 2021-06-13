using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MAWinnerPathFabric : MAPathFabric {

    public class MAPathDescriptionPoint {
        public int stoneIndex;
        public MAPathDescriptionTurn direction;

        public MAPathDescriptionPoint(int stoneIndex, MAPathDescriptionTurn direction) {
            this.stoneIndex = stoneIndex;
            this.direction = direction;
        }
    }



    public MAPathFabric standardPathFabric;

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

    public int currentGeneratedDescriptionPoints = 0;


    private int triesToFreePredictor = 0;

    public int maxTriesToFreePredictor;

    private bool showPrediction = true;


    void Start() {
        this.InstantiatePrognoseFabric();
    }


    void Update() {
        if (this.framesSinceFirstUpdate % this.framesTillUpdate != 0) {
            this.framesSinceFirstUpdate++;
            return;
        }

        if (this.framesSinceFirstUpdate > 5) {
            // Debug.Log("Hello Debugger :)");
        }


        if (this.showPrediction) {

            if (this.isPredictorStuck && this.triesToFreePredictor > this.maxTriesToFreePredictor) {

                this.ShortenDescriptionAndSetPredictor(1);

                this.isPredictorStuck = false;
                this.triesToFreePredictor = 0;
            }
            else {
                this.currentGeneratedDescriptionPoints = 0;

                if (this.LoopPrognoseUpdates()) {
                    this.isPredictorStuck = false;
                    this.triesToFreePredictor = 0;
                    Debug.Log("reached prediction frames!");
                }
                else {
                    this.isPredictorStuck = true;
                    this.triesToFreePredictor++;
                }
            }
        }
        else {
            if (this.isPredictorStuck) {
                this.ShortenDescriptionAndSetPredictor(this.currentGeneratedDescriptionPoints);
            }
        }


        this.showPrediction = !this.showPrediction;


       
    }


    private bool LoopPrognoseUpdates() {

        for (int frameIndex = 0; frameIndex < this.predictionFrames; frameIndex++) {

            if (!this.UpdatePrognoseFabric(frameIndex)) {
                return false;
            }
        }

        return true;
    }



    private void ShortenDescriptionAndSetPredictor(int iteration) {

        for (int iterationIndex = 0; iterationIndex < iteration; iterationIndex++) {

            int descriptionIndex = this.pathDescription.Count - 1;

            MAPathDescriptionPoint prevPoint = null;
            if (descriptionIndex > 0) {
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
        return true;
    }

    private void InstantiatePrognoseFabric() {
        if (this.predictor) {
            GameObject.Destroy(this.predictor.gameObject);
        }
        this.predictor = this.InstantiateNewGenerator();
        this.predictor.enabled = false;
    }

    public void AddPathDescriptionPoint(int stoneIndex, MAPathDescriptionTurn pathDescriptionTurn) {
        this.pathDescription.Add(new MAPathDescriptionPoint(stoneIndex, pathDescriptionTurn));
        this.currentGeneratedDescriptionPoints++;
    }

    public override MAPathFabric GetInstantiatedNewFabric(Transform parent) {
        MAPathFabric newFabric = GameObject.Instantiate(this.standardPathFabric, this.transform.position, this.transform.rotation, parent).GetComponentInChildren<MAPathFabric>();
        newFabric.winnerPathFabric = this;
        return newFabric;
    }
}
