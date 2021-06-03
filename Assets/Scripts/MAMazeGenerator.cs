using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAMazeGenerator : MonoBehaviour {
    public int maxTriesInRowBeforeAbort;

    public GameObject floorStone;
    public GameObject visualCheckStone;

    public float floorStoneSize;

    public float floorDensitiy;

    public GameObject mazeElementContainer;

    public int amountRings;
    public float ringGapDistance;

    [Range(0, 0.25f)]
    public float connectionFrequency;

    [Range(0, 2)]
    public float connectionRandomness;

    [Range(0, 0.5f)]
    public float nextRingOffsetFactor;

    [Range(0.001f, 1f)]
    public float minConnectionDensityAngleIncrease;


    private float currentMinConnectionDensity;


    private List<List<float>> connectionsList = new List<List<float>>();


    //private float ringIndex = 1;
    //private float stoneIndex = 0;


    void Start() {
        this.Init();
    }

    void Init() {
        GameObject.Destroy(this.mazeElementContainer);

        this.mazeElementContainer = null;
        this.mazeElementContainer = new GameObject("Container");
        this.currentMinConnectionDensity = this.minConnectionDensityAngleIncrease;
        this.GenerateCircles();
    }


    void Update() {

        /*'
        if (this.ringIndex <= this.amountRings) {
            float r = this.ringGapDistance * ringIndex;


            float u = 2 * Mathf.PI * r;
            float amountFloorStones = u * this.floorDensitiy;
            //Debug.Log($"amountFloorStones: {amountFloorStones}");


            if (this.stoneIndex < amountFloorStones) {
                // Debug.Log(stoneIndex / amountFloorStones);
                float phi = (this.stoneIndex / amountFloorStones) * 360 * Mathf.Deg2Rad;
                this.DrawStone(r, phi);
                this.stoneIndex++;
            }
            else {
                this.stoneIndex = 0;
                this.ringIndex++;
            }
        }
        */

        if (Input.GetKeyUp(KeyCode.R)) {
            this.Init();
        }

    }


    void GenerateConnections(float r, int ringIndex) {

        float u = 2 * Mathf.PI * r;
        int amountConnections = (int)((u * this.connectionFrequency) + 1);

        float ringOffset = Random.Range(0, ringIndex * this.nextRingOffsetFactor);

        List<float> connections = new List<float>();


        int numberOfUnsuccessfulTries = 0;
        int numberOfConnections = 0;

        int numberOfDensityIncreases = 0;

        for (int connectionIndex = 0; connectionIndex < amountConnections; connectionIndex++) {

            float min = (connectionIndex - this.connectionRandomness) / amountConnections * 360 * Mathf.Deg2Rad + ringOffset;
            float max = (connectionIndex + this.connectionRandomness) / amountConnections * 360 * Mathf.Deg2Rad + ringOffset;

            float phi = Random.Range(min, max) % (2 * Mathf.PI);
            this.DrawStone(r + this.floorStoneSize / 2, phi);
            /*
            if (this.IsConnectionValid(phi, ringIndex)) {
                this.DrawStone(r + this.floorStoneSize / 2, phi);
                connections.Add(phi);


                numberOfConnections++;
            }
            else {
                numberOfUnsuccessfulTries++;
                connectionIndex--;
            }

            if (numberOfUnsuccessfulTries > this.maxTriesInRowBeforeAbort) {
                this.currentMinConnectionDensity += this.minConnectionDensityAngleIncrease;
                numberOfDensityIncreases++;


            }

            if (numberOfDensityIncreases > this.maxTriesInRowBeforeAbort) {
                connectionIndex = amountConnections;
                numberOfUnsuccessfulTries = 0;
            }

            if (numberOfConnections >= amountConnections) {
                connectionIndex = amountConnections;
                break;
            }
            */
        }
        this.connectionsList.Add(connections);
    }


    bool IsConnectionValid(float phi, int ringIndex) {
        if (ringIndex <= 1) {
            return true;
        }

        Debug.Log("checking Phis for ringIndex: " + ringIndex);

        foreach (float connection in this.connectionsList[ringIndex - 1]) {
            // Debug.Log(connection - this.minConnectionDensityAngle);
            //Debug.Log("vs phi:" + phi);
            float minPhi = connection - this.currentMinConnectionDensity;
            float maxPhi = connection + this.currentMinConnectionDensity;

            Debug.Log("minPhi:" + minPhi);
            Debug.Log("maxPhi:" + maxPhi);

            float r = this.ringGapDistance * (ringIndex - 1);
            this.DrawVisualCheck(r, minPhi);
            this.DrawVisualCheck(r, maxPhi);


            if (phi < minPhi || phi > maxPhi) {

                return false;
            }
        }
        return true;
    }


    void GenerateCircles() {
        for (int ringIndex = 0; ringIndex <= this.amountRings; ringIndex++) {
            float r = this.ringGapDistance * (ringIndex + 1);


            this.DrawCircle(r);
            this.GenerateConnections(r, ringIndex);
        }
    }

    void DrawCircle(float r) {
        float u = 2 * Mathf.PI * r;
        float amountFloorStones = u * this.floorDensitiy;

        for (int stoneIndex = 0; stoneIndex < amountFloorStones; stoneIndex++) {
            float phi = (stoneIndex / amountFloorStones) * 360 * Mathf.Deg2Rad;
            this.DrawStone(r, phi);
        }
    }

    void DrawVisualCheck(float r, float phi) {
        Vector3 position = new Vector3(r * Mathf.Cos(phi), 0, r * Mathf.Sin(phi));
        this.DrawStone(position, Quaternion.Euler(0, -phi * Mathf.Rad2Deg, 0) * this.floorStone.transform.rotation);
        GameObject.Instantiate(this.visualCheckStone, position, this.floorStone.transform.rotation, this.mazeElementContainer.transform);
    }

    void DrawStone(float r, float phi) {
        Vector3 position = new Vector3(r * Mathf.Cos(phi), 0, r * Mathf.Sin(phi));
        this.DrawStone(position, Quaternion.Euler(0, -phi * Mathf.Rad2Deg, 0) * this.floorStone.transform.rotation);
    }


    void DrawStone(Vector3 position, Quaternion rotation) {

        GameObject.Instantiate(this.floorStone, position, rotation, this.mazeElementContainer.transform);
    }

    void DrawStone(Vector3 position) {

        GameObject.Instantiate(this.floorStone, position, this.floorStone.transform.rotation, this.mazeElementContainer.transform);
    }


}
