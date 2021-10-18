using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAMazeGenerator : MonoBehaviour {
    public int maxTriesInRowBeforeAbort;

    public GameObject floorStone;
    public GameObject visualCheckStone;
    public MAMazeConnection connectionStone;

    public float floorStoneSize;

    public float floorDensitiy;

    public GameObject circleContainer;
    public GameObject connectionsContainer;


    public int amountRings;
    public float ringGapDistance;

    [Range(0, 0.25f)]
    public float connectionFrequency;

    [Range(0, 0.75f)]
    public float connectionRandomness;

    [Range(0, 0.5f)]
    public float nextRingOffsetFactor;

    public float minConnectionDistance;



    //private float ringIndex = 1;
    //private float stoneIndex = 0;

    private bool newMazeLastFrame = false;
    int framesTillNewMaze = 0;

    void Start() {
        this.Init();
    }

    void Init() {

        foreach (Transform child in this.circleContainer.transform) {
            GameObject.Destroy(child.gameObject);
        }


        foreach (Transform child in this.connectionsContainer.transform) {
            MAMazeConnection mazeConnection = child.gameObject.GetComponent<MAMazeConnection>();
            mazeConnection.isNewStone = false;
            GameObject.Destroy(child.gameObject);
        }

        this.GenerateMaze();
    }


    void Update() {

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
            float radius = r + this.floorStoneSize / 2;


            if (this.IsConnectionValid(phi, radius, ringIndex)) {
                this.DrawConnectionStone(radius, phi, ringIndex);
            }
        }
    }


    bool IsConnectionValid(float phi, float radius, int ringIndex) {
        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(radius * Mathf.Cos(phi), 0, radius * Mathf.Sin(phi)), this.minConnectionDistance);

        foreach (Collider col in hitColliders) {
            if (col.gameObject.GetComponent<MAMazeConnection>()) {
                MAMazeConnection mazeConnection = col.gameObject.GetComponent<MAMazeConnection>();

                if (mazeConnection.isNewStone && mazeConnection.ring == ringIndex - 1) {
                    return false;
                }
            }
        }
        return true;
    }


    void GenerateMaze() {
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
            this.DrawStone(r, phi, this.circleContainer);
        }
    }

    void DrawVisualCheck(float r, float phi) {
        Vector3 position = new Vector3(r * Mathf.Cos(phi), 0, r * Mathf.Sin(phi));
        GameObject.Instantiate(this.visualCheckStone, position, Quaternion.Euler(0, -phi * Mathf.Rad2Deg, 0) * this.floorStone.transform.rotation, this.circleContainer.transform);
    }

    void DrawConnectionStone(float r, float phi, int ring) {
        Vector3 position = new Vector3(r * Mathf.Cos(phi), 0, r * Mathf.Sin(phi));
        Quaternion rotation = Quaternion.Euler(0, -phi * Mathf.Rad2Deg, 0) * this.floorStone.transform.rotation;
        this.DrawConnectionStone(position, rotation, ring);
    }

    void DrawStone(float r, float phi, GameObject parent) {
        Vector3 position = new Vector3(r * Mathf.Cos(phi), 0, r * Mathf.Sin(phi));
        this.DrawStone(position, Quaternion.Euler(0, -phi * Mathf.Rad2Deg, 0) * this.floorStone.transform.rotation, parent);
    }

    void DrawConnectionStone(Vector3 position, Quaternion rotation, int ring) {
        this.connectionStone.isNewStone = true;
        GameObject.Instantiate(this.connectionStone, position, rotation, this.connectionsContainer.transform);
    }

    void DrawStone(Vector3 position, Quaternion rotation, GameObject parent) {
        GameObject.Instantiate(this.floorStone, position, rotation, parent.transform);
    }

    void DrawStone(Vector3 position) {
        GameObject.Instantiate(this.floorStone, position, this.floorStone.transform.rotation, this.circleContainer.transform);
    }
}
