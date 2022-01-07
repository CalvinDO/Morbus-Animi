using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MAJackUpper : MonoBehaviour {


    public Transform handUpMaxPosition;
    public Transform handDownMinPosition;


    public MAJackUpEnoughSpaceChecker enoughSpaceTrigger;

    private MACharacterController characterController;

    private Vector3 attachedEdgePoint;
    private Vector3 attachedStandingPoint;


    [Range(0, 10)]
    public float obstacleCollectorRadius;

    [Range(0, 180)]
    public float maxLookAtAngle;

    [Range(0, 1)]
    public float catpassSlerpFactor;

    [Range(0, 1)]
    public float hangSlerpFactor;

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
    private bool isReadyToHangLift = false;
    private bool isLiftingUp = false;


    private bool isHeightReached = false;

    [Range(0, 1)]
    public float moveXYExtraMagnitude;

    public Rig armRig;

    public Transform leftHandTarget;
    public Transform rightHandTarget;

    public Transform leftHand;
    public Transform rightHand;

    [Range(0, 0.5f)]
    public float hangEdgeDistance;


    void Start() {


        this.characterController = this.transform.root.GetComponent<MACharacterController>();
    }


    // Update is called once per frame
    void Update() {

        /*
        if (Input.GetKey(KeyCode.Backspace)) {
            this.DiscardLedge();
            return;
        }

        */

        if (this.isAttached) {
            this.CheckCharacterInputTowardsLedge();
        }

        this.handlePush();

    }


    public Collider[] GetCurrentObstacles() {

        Collider[] physicOverlappedObstacles = Physics.OverlapSphere(this.attachedStandingPoint, this.obstacleCollectorRadius, 13);


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

        Vector3 extraMagnitudeXZProject = Vector3.ProjectOnPlane(closestPoint - characterController.transform.position, Vector3.up);





        Vector3 standingPoint = closestPoint;
        standingPoint += extraMagnitudeXZProject.normalized * this.moveXYExtraMagnitude;

        Debug.Log(standingPoint);
        Debug.Log(closestPoint);



        Vector3 sameYCharacterStandingPoint = this.transform.position;
        sameYCharacterStandingPoint.y = standingPoint.y;




        if (standingPoint.y > this.handDownMinPosition.position.y && standingPoint.y < this.handUpMaxPosition.position.y - this.handUpMaxTolerance) {

            Vector3 ledgeDirectionSameYCharPos = standingPoint - sameYCharacterStandingPoint;

            if (Vector3.Angle(this.transform.forward, ledgeDirectionSameYCharPos) < this.maxLookAtAngle) {

                if (this.IsCharacterInputTowardsLedge(standingPoint)) {

                    if (this.enoughSpaceTrigger.GetSpaceFreeAt(standingPoint)) {

                        Debug.DrawLine(this.handUpMaxPosition.position, standingPoint, Color.green);

                        this.attachedEdgePoint = closestPoint;
                        this.DecideCatpassOrHang(standingPoint, sameYCharacterStandingPoint);


                        return;
                    }
                    else {
                        Debug.DrawLine(this.handUpMaxPosition.position, standingPoint, Color.yellow);
                    }
                }
            }
            else {
                Debug.DrawLine(this.handUpMaxPosition.position, standingPoint, Color.red);

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


        if (this.characterController.normalizedSummedInput == Vector3.zero) {
            return false;
        }

        if (Vector3.Angle(this.characterController.normalizedSummedInput, ledgeDirectionSameYCharPos) < this.maxLookAtAngle) {
            return true;
        }

        return false;

    }


    private void OnDrawGizmos() {


        if (this.isHanging) {
            //Debug.Log("hang: " + this.isHanging + "  |||   catpass: " + this.isCatpassing);
            Gizmos.DrawWireSphere(this.attachedStandingPoint, 0.2f);
            return;
        }

        if (this.isCatpassing) {
            // Debug.Log("hang: " + this.isHanging + "  |||   catpass: " + this.isCatpassing);

            Gizmos.DrawSphere(this.attachedStandingPoint, 0.2f);
        }

    }

    private void DecideCatpassOrHang(Vector3 closestPoint, Vector3 sameYCharacterClosestPoint) {

        Ray floorRay = new Ray(sameYCharacterClosestPoint, Vector3.down);

        Debug.DrawRay(floorRay.origin, floorRay.direction, Color.cyan);

        LayerMask mask = LayerMask.GetMask("Default", "MA_NavMesh", "Wall");
        Debug.Log(mask.value);

        if (Physics.Raycast(floorRay, out _, this.handUpMaxPosition.localPosition.y, mask)) {

            this.Catpass(closestPoint);

            Debug.Log("catpass!");
        }
        else {

            this.Hang(closestPoint);

            Debug.Log("hang!");
        }




        this.RotateCharacterTowardsAttachedPoint();

        this.characterController.SetPerformingSoloJumpNRunMove(true);

        this.SetHandsOnLedge(this.attachedEdgePoint);

    }


    private void SetHandsOnLedge(Vector3 closestPoint) {
        this.armRig.weight = 1;

        this.leftHandTarget.position = closestPoint;

        this.rightHandTarget.position = closestPoint;


        this.leftHand.rotation = Quaternion.identity;
        this.rightHand.rotation = Quaternion.identity;
    }

    private void FreeHandsFromLedge() {
        this.armRig.weight = 0;
    }

    private void RotateCharacterTowardsAttachedPoint() {
        this.characterController.physicalBody.transform.LookAt(new Vector3(this.attachedStandingPoint.x, this.characterController.transform.position.y, this.attachedStandingPoint.z));
    }

    private void DiscardLedge() {

        this.isHanging = false;
        this.isCatpassing = false;
        this.isAttached = false;

        this.isLiftingUp = false;

        this.characterController.rb.isKinematic = false;


        this.isHeightReached = false;

        this.isUpjackingEnabled = true;
        this.isReadyToHangLift = false;

        this.attachedStandingPoint = Vector3.zero;
        this.attachedEdgePoint = Vector3.zero;

        this.characterController.movementEnabled = true;


        this.FreeHandsFromLedge();

    }



    private void Hang(Vector3 closestPoint) {

        this.isHanging = true;
        this.isCatpassing = false;
        this.isAttached = true;

        this.attachedStandingPoint = closestPoint;

        this.isUpjackingEnabled = false;

        this.characterController.animator.SetTrigger("Hang");

        this.characterController.rb.isKinematic = true;
    }


    private void ControlHang() {

        if (this.isLiftingUp) {
            return;
        }

        if (this.isHanging) {

            this.LerpCharacterToHangPos();


            if (!this.isUpjackingEnabled) {

                if (!this.characterController.directionInputExists) {

                    this.isReadyToHangLift = true;
                }

                if (this.isReadyToHangLift) {
                    if (this.characterController.directionInputExists) {
                        this.isUpjackingEnabled = true;
                    }
                }
            }
        }

    }


    private void LerpCharacterToHangPos() {

        Vector3 hangPos = new Vector3(this.attachedEdgePoint.x, (this.attachedStandingPoint - this.handUpMaxPosition.localPosition).y, this.attachedEdgePoint.z);

        Vector3 hangPosToCharXZDirection = this.characterController.transform.position - hangPos;
        hangPosToCharXZDirection.y = 0;

        hangPos += hangPosToCharXZDirection.normalized * this.hangEdgeDistance;

        this.characterController.transform.position = Vector3.Lerp(characterController.transform.position, hangPos, this.hangSlerpFactor);


    }

    private void Catpass(Vector3 closestPoint) {

        this.isHanging = false;
        this.isCatpassing = true;
        this.isAttached = true;

        this.attachedStandingPoint = closestPoint;


        this.isUpjackingEnabled = true;

        this.characterController.rb.isKinematic = true;


        this.characterController.animator.SetTrigger("Catpass");
    }

    private void CheckCharacterInputTowardsLedge() {


        if (this.IsCharacterInputTowardsLedge(this.attachedStandingPoint)) {

            if (this.isUpjackingEnabled) {

                if (!this.isLiftingUp) {
                    this.SetLiftUpState();
                }
            }
        }
        else {
            if (this.characterController.directionInputExists) {
                if (this.isHanging) {

                    if (!this.isLiftingUp) {

                        if (this.isUpjackingEnabled) {
                            this.DropFromHang();
                        }
                    }

                }
            }
        }
    }

    private void DropFromHang() {
        this.characterController.animator.SetTrigger("DropFromHang");
        this.characterController.SetPerformingSoloJumpNRunMove(false);
        this.DiscardLedge();
    }

    private void SetLiftUpState() {

        this.isLiftingUp = true;

        if (this.isHanging) {
            this.characterController.animator.SetTrigger("HangLiftUp");
            this.characterController.animator.ResetTrigger("Hang");
        }
    }

    private void SetHeightReached() {
        this.isHeightReached = true;
    }

    private void SetJackUpCompleted() {

        this.characterController.animator.ResetTrigger("HangLiftUp");
        this.characterController.animator.ResetTrigger("Catpass");

        this.characterController.SetPerformingSoloJumpNRunMove(false);

        this.DiscardLedge();
    }

    private void LiftUpCatpass() {
        Vector3 characterArrivedPosition = new Vector3(this.characterController.transform.position.x, this.attachedStandingPoint.y + this.liftUpExtraThreshhold, this.characterController.transform.position.z);

        this.characterController.transform.position = Vector3.Slerp(this.characterController.transform.position, characterArrivedPosition, this.catpassSlerpFactor);
    }


    private void LiftUpHang() {
        Vector3 characterArrivedPosition = new Vector3(this.characterController.transform.position.x, this.attachedStandingPoint.y + this.liftUpExtraThreshhold, this.characterController.transform.position.z);

        this.characterController.transform.position = Vector3.Slerp(this.characterController.transform.position, characterArrivedPosition, this.hangSlerpFactor);
    }



    private void LiftPlanar() {
        Vector3 characterArrivedPosition = this.attachedStandingPoint;


        this.characterController.transform.position = Vector3.Lerp(this.characterController.transform.position, characterArrivedPosition, this.catpassSlerpFactor);
    }

    private void FixedUpdate() {

        this.ControlHang();

        if (!this.isAttached) {
            return;
        }

        this.SetHandsOnLedge(this.attachedEdgePoint);

        if (!this.isUpjackingEnabled) {
            return;
        }



        if (this.attachedStandingPoint == Vector3.zero) {
            Debug.LogError("Character attached but attached Point is Zero!");
        }



        if (this.isLiftingUp) {

            if (this.isCatpassing) {
                this.LiftUpCatpass();
            }

            if (this.isHanging) {
                this.LiftUpHang();
            }

        }

        if (this.isHeightReached) {
            this.LiftPlanar();
        }


        if (this.attachedStandingPoint != Vector3.zero) {

            if (this.characterController.transform.position.y >= this.attachedStandingPoint.y) {
                this.SetHeightReached();
            }

        }

        Vector3 XZDistanceToClosestPointVector = this.attachedStandingPoint - new Vector3(this.characterController.transform.position.x, this.attachedStandingPoint.y, this.characterController.transform.position.z);
        float xzdistanceToClosestPoint = XZDistanceToClosestPointVector.magnitude;

        if (xzdistanceToClosestPoint < this.xzDistanceToClosestPointThreshhold) {
            this.SetJackUpCompleted();
        }
    }

    private void handlePush() {
        // fix for conflict with pushing mechanic
        if (Input.GetKey("e")) {
            this.isUpjackingEnabled = false;
        }
        if (Input.GetKeyUp("e")) {
            this.isUpjackingEnabled = true;
        }
    }
}
