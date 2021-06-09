using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MAPathFabricDirection {
    Right = 0,
    Outwards = 1,
    Left = 2,
    Inwards = 3
}

public enum MACrashDirection {
    Left = 0,

}
public class MAPathFabric : MonoBehaviour {

    [Range(0, 1)]
    public float speed;

    public float angularSpeed;
    private float angularVelocity;

    public GameObject pathFabricRotator;

    public MAPathFabricDirection direction;

    public GameObject connectionStone;
    public GameObject generatedStonesContainer;

    [Range(0, 30)]
    public int framesTillDirectionChange;

    [Range(0, 30)]
    public float framesTillDirectionChangeAfterCrash;

    [Range(0, 100)]
    public int minFramesTillDirectionChange;

    [Range(0, 100)]
    public int maxFramesTillDirectionChange;

    public GameObject distanceTriggerLeft;
    public GameObject distanceTriggerFront;
    public GameObject distanceTriggerRight;

    [Range(0, 6)]
    public float collisionDetectionSize;


    private int framesSinceLastChange = 0;
    private float framesSinceLastCrash = 0;

    private bool recentlyCrashed = false;
    public bool fatalCrashed = false;

    private int triesToExit = 0;
    public int maxTriesToExit;

    public int framesTillStoneDrop;
    private int framesSinceLastStoneDrop = 0;




    [Range(0, 1)]
    public float reproductionChance;


    [Range(0, 10)]
    public int streightLinerFramesLimit;

    private int streightLinerFramesCounter = 0;

    private int streightLinerStreak;




    void Start() {
    }

    private void Initialize(Vector3 position, Quaternion rotation) {

        this.transform.position = position;
        this.transform.rotation = rotation;

    }

    // Update is called once per frame
    void Update() {

        this.CalculateAngularVelocity();

        this.SetRandomTurnFrequency();


        if (!this.fatalCrashed) {
            if (!this.Move()) {
                this.fatalCrashed = true;
                this.gameObject.SetActive(false);
            }
        }
    }



    public void SetRandomTurnFrequency() {

    }

    public void CalculateAngularVelocity() {
        this.angularVelocity = this.angularSpeed / this.transform.position.magnitude;
    }


    private bool RandomDirectionChange(bool IsInstantanious) {

        if (this.framesSinceLastChange > this.framesTillDirectionChange || IsInstantanious) {

            int randomRotChange = this.GetWeightedRandomDirectionChange();

            this.direction += randomRotChange;
            this.ModuloDirection();


            if (randomRotChange != 0) {
                this.transform.Rotate(randomRotChange == 1 ? new Vector3(0, -90) : new Vector3(0, 90));
            }


            if (this.IsCrashing()) {
                if (!this.FindCrashExit()) {
                    return false;
                }
                this.ModuloDirection();
            }

            if (Random.Range(0f, 1f) < this.reproductionChance) {
                this.InstantiateNewGenerator(180, -2);
            }

            this.framesSinceLastChange = 0;

            this.framesTillDirectionChange = Random.Range(this.minFramesTillDirectionChange, this.maxFramesTillDirectionChange);
        }

        this.framesSinceLastChange++;

        return true;
    }

    private int GetWeightedRandomDirectionChange() {
        float random = Random.Range(0f, 1f);

        if (random < MAPathManager.directionalWeightStrengthStatic) {

            Debug.Log("directionalWeightStatic: " + MAPathManager.directionalWeightStatic);

            int destinatedWeight =MAPathManager.directionalWeightStatic - this.direction;

            int moduloedDirection = this.ModuloDirection(destinatedWeight);
            if (destinatedWeight == 0) {
                return 0;
            }
            return (int)Mathf.Sign(destinatedWeight);
        }

        return GetRandomDirectionChange();
    }

    public static int GetRandomDirectionChange() {
        return ((int)Random.Range(0, 2)) == 1 ? -1 : 1;
    }

    private void InstantiateNewGenerator(float angle, int directionChange) {
        GameObject newGenerator = GameObject.Instantiate(new GameObject("PathGenerator"));
        GameObject newRotator = GameObject.Instantiate(new GameObject("PathRotator"), newGenerator.transform);
        GameObject newGeneratedStonesContainer = GameObject.Instantiate(new GameObject("GeneratedStonesContainer"), newGenerator.transform);

        MAPathFabric newFabric = GameObject.Instantiate(this.gameObject, this.transform.position, this.transform.rotation, newRotator.transform).GetComponentInChildren<MAPathFabric>();

        newFabric.Initialize(this.transform.position, this.transform.rotation);

        newFabric.transform.Rotate(new Vector3(0, angle, 0));
        newFabric.direction = this.direction;
        newFabric.direction += directionChange;

        newFabric.ModuloDirection();

        newFabric.pathFabricRotator = newRotator;
        newFabric.generatedStonesContainer = newGeneratedStonesContainer;
    }

    private int ModuloDirection(int old) {
        int modulo = old % 4;

        return modulo < 0 ? 4 - modulo : modulo;
    }

    public static MAPathFabricDirection ModuloDirection(MAPathFabricDirection old) {
        old = (MAPathFabricDirection)((int)old % 4);


        if ((int)old == -1) {
            old = MAPathFabricDirection.Inwards;
        }

        if ((int)old == -2) {
            old = MAPathFabricDirection.Left;
        }

        if ((int)old == -3) {
            old = MAPathFabricDirection.Outwards;
        }

        if ((int)old == -4) {
            old = MAPathFabricDirection.Right;
        }

        return old;
    }
    private void ModuloDirection() {
        this.direction = (MAPathFabricDirection)((int)this.direction % 4);


        if ((int)this.direction == -1) {
            this.direction = MAPathFabricDirection.Inwards;
        }

        if ((int)this.direction == -2) {
            this.direction = MAPathFabricDirection.Left;
        }

        if ((int)this.direction == -3) {
            this.direction = MAPathFabricDirection.Outwards;
        }

        if ((int)this.direction == -4) {
            this.direction = MAPathFabricDirection.Right;
        }

        return;
    }

    public bool Move() {

        if (this.IsCrashing()) {
            if (!this.FindCrashExit()) {
                if (this.triesToExit > this.maxTriesToExit) {
                  // Debug.Log("CRASH because no Exit found!");
                    return false;
                }

                this.triesToExit += 1;
            }



            this.recentlyCrashed = true;

        }

        this.ModuloDirection();



        if (this.recentlyCrashed) {
            if (this.framesSinceLastCrash > this.framesTillDirectionChangeAfterCrash) {

                this.framesSinceLastCrash = 0;
                this.recentlyCrashed = false;
            }
            this.framesSinceLastCrash++;
        }
        else {
            if (!this.RandomDirectionChange(false)) {
                return false;
            }
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

        if (this.framesSinceLastStoneDrop % this.framesTillStoneDrop == 0) {
            this.DrawStone();
            this.framesSinceLastStoneDrop = 0;
        }
        this.framesSinceLastStoneDrop++;

        return true;
    }

    private void DrawStone() {
        GameObject.Instantiate(this.connectionStone, this.transform.position, this.transform.rotation, this.generatedStonesContainer.transform);
    }

    private bool IsCrashing() {

        return Physics.OverlapBox(this.distanceTriggerFront.transform.position, new Vector3(1, 1, 1) * this.collisionDetectionSize, this.transform.rotation).Length > 0;
    }


    private Vector3 GetRandomCrashRotation() {
        if (Random.Range(0, 2) == 1) {
            return new Vector3(0, 90);
        }
        return new Vector3(0, -90);
    }

    private void increaseDirection(int increase) {
        this.direction += increase;
        this.ModuloDirection();
    }

    private bool IsLeftFree() {
        return !(Physics.OverlapSphere(this.distanceTriggerLeft.transform.position, 4).Length > 0);
    }

    private bool IsRightFree() {
        return !(Physics.OverlapSphere(this.distanceTriggerRight.transform.position, 4).Length > 0);
    }

    private bool FindCrashExit() {
        if (this.IsRightFree()) {
            this.transform.Rotate(new Vector3(0, 90));
            this.direction -= 1;
        }
        else if (this.IsLeftFree()) {
            this.transform.Rotate(new Vector3(0, -90));
            this.direction += 1;
        }
        else {
            return false;
        }
        return true;
    }
}
