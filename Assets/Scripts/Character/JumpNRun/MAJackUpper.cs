using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAJackUpper : MonoBehaviour {


    public Transform handUpMaxPosition;
    public Transform handDownMinPosition;


    public MAJackUpEnoughSpaceChecker enoughSpaceTrigger;

    private MACharacterController characterController;

    private Vector3 attachedPoint;


    [Range(0, 10)]
    public float obstacleCollectorRadius;

    [Range(0, 180)]
    public float maxLookAtAngle;

    [Range(0, 1)]
    public float liftUpSlerpFactor;
    [Range(0, 1)]
    public float liftUpExtraThreshhold;
    [Range(0, 1)]
    public float handUpMaxTolerance;

    [Range(0, 1)]
    public float xzDistanceToClosestPointThreshhold;



    public bool isHanging = false;
    public bool isCatpassing = false;
    public bool isAttached = false;


    private bool isUpjackingEnabled = true;
    private bool isLiftingUp = false;


    private bool isHeightReached = false;

    [Range(0, 1)]
    public float moveXYExtraMagnitude;


    void Start() {


        this.characterController = this.transform.root.GetComponent<MACharacterController>();
    }


    // Update is called once per frame
    void Update() {

        if (Input.GetKey(KeyCode.Backspace)) {
            this.DiscardLedge();
            return;
        }

        if (this.isAttached) {
            this.CheckCharacterInputTowardsLedge();
        }

    }


    public Collider[] GetCurrentObstacles() {

        Collider[] physicOverlappedObstacles = Physics.OverlapSphere(this.attachedPoint, this.obstacleCollectorRadius);


        List<Collider> filteredColliders = new List<Collider>();

        foreach (Collider obstacle in physicOverlappedObstacles) {
            if (!obstacle.isTrigger) {
                if (!obstacle.transform.root.GetComponent<MACharacterController>()) {

                    filteredColliders.Add(obstacle);
                }
            }
        }

        return filteredColliders.ToArray();
    }


    private void OnTriggerStay(Collider ledgeCandidateCollider) {

        if (ledgeCandidateCollider.isTrigger) {
            return;
        }

        if (!this.isUpjackingEnabled) {
            return;
        }

        if (this.isAttached) {
            return;
        }

        if (this.characterController.IsPerformingSoloJumpNRunMove) {
            return;
        }


        Vector3 closestPoint = Vector3.zero;

        if (ledgeCandidateCollider.GetType() == typeof(BoxCollider) || ledgeCandidateCollider.GetType() == typeof(SphereCollider)) {
            closestPoint = ledgeCandidateCollider.ClosestPoint(this.handUpMaxPosition.position);
        }
        else if (ledgeCandidateCollider.GetType() == typeof(MeshCollider)) {

            MeshCollider currentMeshCollider = (MeshCollider)ledgeCandidateCollider;

            if (currentMeshCollider.convex) {
                closestPoint = ledgeCandidateCollider.ClosestPoint(this.handUpMaxPosition.position);
            }
        }

        if (closestPoint == Vector3.zero) {
            return;
        }


        Vector3 sameYCharacterClosestPoint = this.transform.position;
        sameYCharacterClosestPoint.y = closestPoint.y;




        if (closestPoint.y > this.handDownMinPosition.position.y && closestPoint.y < this.handUpMaxPosition.position.y - this.handUpMaxTolerance) {


            Vector3 ledgeDirectionSameYCharPos = closestPoint - sameYCharacterClosestPoint;

            if (Vector3.Angle(this.transform.forward, ledgeDirectionSameYCharPos) < this.maxLookAtAngle) {

                if (this.IsCharacterInputTowardsLedge(closestPoint)) {

                    if (this.enoughSpaceTrigger.GetSpaceFreeAt(closestPoint)) {
                        Debug.DrawLine(this.handUpMaxPosition.position, closestPoint, Color.green);

                        this.DecideCatpassOrHang(closestPoint, sameYCharacterClosestPoint);

                        return;
                    }
                    else {
                        Debug.DrawLine(this.handUpMaxPosition.position, closestPoint, Color.yellow);
                    }
                }
            }
            else {
                Debug.DrawLine(this.handUpMaxPosition.position, closestPoint, Color.red);

            }
        }

        this.DiscardLedge();
    }

    private bool IsCharacterInputTowardsLedge(Vector3 closestPoint) {

        if (closestPoint == Vector3.zero) {
            Debug.LogError("closest point is zero!");
        }


        Vector3 sameYCharacterClosestPoint = this.transform.position;
        sameYCharacterClosestPoint.y = closestPoint.y;
        Vector3 ledgeDirectionSameYCharPos = closestPoint - sameYCharacterClosestPoint;

        Debug.DrawLine(this.characterController.transform.position, this.characterController.transform.TransformPoint(this.characterController.normalizedSummedInput));


        if (Vector3.Angle(this.characterController.normalizedSummedInput, ledgeDirectionSameYCharPos) < this.maxLookAtAngle) {
            return true;
        }

        return false;

    }

    private void OnDrawGizmos() {


        if (this.isHanging) {
            //Debug.Log("hang: " + this.isHanging + "  |||   catpass: " + this.isCatpassing);
            Gizmos.DrawWireSphere(this.attachedPoint, 0.2f);
            return;
        }

        if (this.isCatpassing) {
            // Debug.Log("hang: " + this.isHanging + "  |||   catpass: " + this.isCatpassing);

            Gizmos.DrawSphere(this.attachedPoint, 0.2f);
        }

    }

    private void DecideCatpassOrHang(Vector3 closestPoint, Vector3 sameYCharacterClosestPoint) {

        Ray floorRay = new Ray(sameYCharacterClosestPoint, Vector3.down);


        if (Physics.Raycast(floorRay, out _, this.handUpMaxPosition.localPosition.y, 13)) {

            this.Catpass(closestPoint);
        }
        else {

            this.Hang(closestPoint);
        }

        this.characterController.SetPerformingSoloJumpNRunMove(true);
    }



    private void DiscardLedge() {

        this.isHanging = false;
        this.isCatpassing = false;
        this.isAttached = false;

        this.isLiftingUp = false;

        this.characterController.rb.isKinematic = false;


        this.isHeightReached = false;

    }



    public void Hang(Vector3 closestPoint) {

        this.isHanging = true;
        this.isCatpassing = false;
        this.isAttached = true;

        this.attachedPoint = closestPoint;
    }

    public void Catpass(Vector3 closestPoint) {

        this.isHanging = false;
        this.isCatpassing = true;
        this.isAttached = true;

        this.attachedPoint = closestPoint;
    }

    public void CheckCharacterInputTowardsLedge() {

        if (this.IsCharacterInputTowardsLedge(this.attachedPoint)) {
            this.SetLiftUpState();
        }
    }



    private void SetLiftUpState() {

        this.isLiftingUp = true;
        this.characterController.rb.isKinematic = true;
    }

    private void SetHeightReached() {
        this.isHeightReached = true;
    }

    private void SetJackUpCompleted() {

        this.characterController.SetPerformingSoloJumpNRunMove(false);
        this.DiscardLedge();
    }

    private void LiftUp() {
        Vector3 characterArrivedPosition = new Vector3(this.characterController.transform.position.x, this.attachedPoint.y + this.liftUpExtraThreshhold * 10, this.characterController.transform.position.z);

        this.characterController.transform.position = Vector3.Lerp(characterController.transform.position, characterArrivedPosition, this.liftUpSlerpFactor);
    }

    private void MoveXY() {
        Vector3 characterArrivedPosition = this.attachedPoint;
        characterArrivedPosition += (this.attachedPoint - characterController.transform.position).normalized * this.moveXYExtraMagnitude;


        this.characterController.transform.position = Vector3.Lerp(characterController.transform.position, characterArrivedPosition, this.liftUpSlerpFactor);
    }

    private void FixedUpdate() {

        if (!this.isUpjackingEnabled) {
            return;
        }

        if (!this.isAttached) {
            return;
        }


        if (this.attachedPoint == Vector3.zero) {
            Debug.LogError("Character attached but attached Point is Zero!");
        }


        if (this.isLiftingUp) {
            this.LiftUp();
        }

        if (this.isHeightReached) {
            Debug.Log("moveXZ");
            this.MoveXY();
        }


        if (this.attachedPoint != Vector3.zero) {

            if (this.characterController.transform.position.y >= this.attachedPoint.y) {
                this.SetHeightReached();
            }

        }

        Vector3 XZDistanceToClosestPointVector = this.attachedPoint - new Vector3(this.characterController.transform.position.x, this.attachedPoint.y, this.characterController.transform.position.z);
        float xzdistanceToClosestPoint = XZDistanceToClosestPointVector.magnitude;

        if (xzdistanceToClosestPoint < this.xzDistanceToClosestPointThreshhold) {
            this.SetJackUpCompleted();
        }
    }
}
