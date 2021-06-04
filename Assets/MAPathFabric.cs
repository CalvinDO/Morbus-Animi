using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MAPathFabricDirection {
    Right = 0,
    Outwards = 1,
    Left = 2,
    Inwards = 3
}

public class MAPathFabric : MonoBehaviour {

    [Range(0, 1)]
    public float speed;

    public float angularSpeed;
    private float angularVelocity;

    public GameObject pathFabricRotator;

    public MAPathFabricDirection direction;

    public MAMazeConnection connectionStone;

    [Range(0, 10)]
    public float timeTillDirectionChange;

    [Range(0, 2)]
    public float timeTillDirectionChangeAfterCrash;


    public GameObject distanceTrigger;

    private float timeSinceLastChange = 0;
    private float timeSinceLastCrash = 0;

    private bool recentlyCrashed = false;
    public bool fatalCrashed = false;

    private int triesToExit = 0;
    public int maxTriesToExit;


    void Start() {
    }

    private void Initialize(Vector3 position, Quaternion rotation) {

        this.transform.position = position;
        this.transform.rotation = rotation;


    }

    // Update is called once per frame
    void Update() {

        this.CalculateAngularVelocity();

        if (!this.fatalCrashed) {
            if (!this.Move()) {
                this.fatalCrashed = true;
                GameObject.Destroy(this.gameObject);
            }
        }


    }


    private void CalculateAngularVelocity() {
        this.angularVelocity = this.angularSpeed / this.transform.position.magnitude;
    }


    private void RandomDirectionChange() {

        if (this.timeSinceLastChange > this.timeTillDirectionChange) {

            int randomRotChange = this.GetRandomDirectionChange();
            this.direction += randomRotChange;
            this.ModuloDirection();



            this.transform.Rotate(randomRotChange == 1 ? new Vector3(0, -90) : new Vector3(0, 90));

            this.GenerateNewFabric(180, 2);
           // this.GenerateNewFabric(90, 1);
           // this.GenerateNewFabric(90, -1);
            this.GenerateNewFabric(90, -1);
           // this.GenerateNewFabric(-90, -1);

            this.timeSinceLastChange = 0;
            Debug.Log("RandomDirectionChange!");
        }

        this.timeSinceLastChange += Time.deltaTime;
    }

    private void GenerateNewFabric(float angle, int directionChange) {
        GameObject newRotator = GameObject.Instantiate(new GameObject("PathRotator"));
        MAPathFabric newFabric = GameObject.Instantiate(this.gameObject, this.transform.position, this.transform.rotation, newRotator.transform).GetComponentInChildren<MAPathFabric>();
        newFabric.Initialize(this.transform.position, this.transform.rotation);




        newFabric.transform.Rotate(new Vector3(0, angle, 0));
        newFabric.direction = this.direction;
        newFabric.direction += directionChange;

        newFabric.ModuloDirection();

        newFabric.pathFabricRotator = newRotator;

    }

    private void ModuloDirection() {
        if ((int)this.direction > 3) {
            this.direction = 0;
        }

        if (this.direction < 0) {
            this.direction = MAPathFabricDirection.Inwards;
        }
    }

    private bool Move() {

        if (this.IsCrashing()) {
            if (!this.FindCrashExit()) {
                if (this.triesToExit > this.maxTriesToExit) {
                    Debug.Log("CRASH because no Exit found!");
                    return false;
                }

                triesToExit += 1;
            }



            Debug.Log("Crashing!");
            this.recentlyCrashed = true;

        }


        if (this.recentlyCrashed) {
            if (this.timeSinceLastCrash > this.timeTillDirectionChangeAfterCrash) {

                this.timeSinceLastCrash = 0;
                this.recentlyCrashed = false;
            }
            this.timeSinceLastCrash += Time.deltaTime;
        }
        else {
            this.RandomDirectionChange();
        }


        switch (this.direction) {
            case MAPathFabricDirection.Right:
                Vector3 rotationRight = new Vector3(0, this.angularVelocity);
                this.pathFabricRotator.transform.Rotate(rotationRight);
                break;
            case MAPathFabricDirection.Outwards:
                this.transform.position += this.transform.position.normalized * speed;
                break;
            case MAPathFabricDirection.Left:
                Vector3 rotationLeft = new Vector3(0, -this.angularVelocity);
                this.pathFabricRotator.transform.Rotate(rotationLeft);
                break;
            case MAPathFabricDirection.Inwards:
                this.transform.position -= this.transform.position.normalized * speed;
                break;
            default:
                Debug.Log("CRASH because unknown direction!");
                return false;
        }

        GameObject.Instantiate(this.connectionStone, this.transform.position, this.pathFabricRotator.transform.rotation);

        return true;
    }

    private bool IsCrashing() {
        return Physics.OverlapSphere(this.distanceTrigger.transform.position, 2).Length > 0;
    }


    private Vector3 GetRandomCrashRotation() {
        if (Random.Range(0, 2) == 1) {
            this.increaseDirection(-1);
            return new Vector3(0, 90);
        }
        this.increaseDirection(1);
        return new Vector3(0, -90);
    }

    private void increaseDirection(int increase) {
        this.direction += increase;
        this.ModuloDirection();
    }

    private bool FindCrashExit() {
        Quaternion oldRotation = this.transform.rotation;

        Vector3 firstRotation = this.GetRandomCrashRotation();
        this.transform.Rotate(firstRotation);

        if (this.IsCrashing()) {
            this.transform.Rotate(new Vector3(0, 180));
            this.direction += 2;
            this.ModuloDirection();

            if (this.IsCrashing()) {
                this.transform.rotation = oldRotation;
                return false;
            }
        }
        this.transform.rotation = oldRotation;
        return true;
    }

    private int GetRandomDirectionChange() {
        return Random.Range(0, 2) == 1 ? 1 : -1;
    }
}
