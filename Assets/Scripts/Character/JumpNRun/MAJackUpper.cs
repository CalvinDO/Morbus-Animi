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

    [SerializeField]
    private Transform attachedEdgePoint;
    [SerializeField]
    private Transform attachedStandingPoint;

    private GameObject attachedGameObject;

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

    [Range(0, 0.5f)]
    public float handSpreadDistance;

    public AudioSource audioSource;
    public AudioClip hangClip;

    void Start() {


        this.characterController = this.transform.GetComponentInParent<MACharacterController>();
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

        Collider[] physicOverlappedObstacles = Physics.OverlapSphere(this.attachedStandingPoint.position, this.obstacleCollectorRadius, 13);


        List<Collider> filteredColliders = new List<Collider>();

        foreach (Collider obstacle in physicOverlappedObstacles) {
            if (!obstacle.isTrigger) {
                if (!obstacle.transform.GetComponentInParent<MACharacterController>()) {

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

        if (ledgeCandidateCollider.CompareTag("NoJackup")) {
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


        Vector3 sameYCharacterStandingPoint = this.transform.position;
        sameYCharacterStandingPoint.y = standingPoint.y;



        if (standingPoint.y > this.handDownMinPosition.position.y && standingPoint.y < this.handUpMaxPosition.position.y - this.handUpMaxTolerance) {

            Vector3 ledgeDirectionSameYCharPos = standingPoint - sameYCharacterStandingPoint;

            if (Vector3.Angle(this.transform.forward, ledgeDirectionSameYCharPos) < this.maxLookAtAngle) {

                if (this.IsCharacterInputTowardsLedge(standingPoint)) {

                    if (this.enoughSpaceTrigger.GetSpaceFreeAt(standingPoint)) {

                        Debug.DrawLine(this.handUpMaxPosition.position, standingPoint, Color.green);

                        this.attachedEdgePoint.position = closestPoint;

                        this.attachedGameObject = ledgeCandidateCollider.gameObject;

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
            Gizmos.DrawWireSphere(this.attachedStandingPoint.position, 0.2f);
            return;
        }

        if (this.isCatpassing) {
            // Debug.Log("hang: " + this.isHanging + "  |||   catpass: " + this.isCatpassing);

            Gizmos.DrawSphere(this.attachedStandingPoint.position, 0.2f);
        }

    }

    private void DecideCatpassOrHang(Vector3 closestPoint, Vector3 sameYCharacterClosestPoint) {

        Ray floorRay = new Ray(sameYCharacterClosestPoint, Vector3.down);

        Debug.DrawRay(floorRay.origin, floorRay.direction, Color.cyan);

        LayerMask mask = LayerMask.GetMask("Default", "MA_NavMesh", "Wall", "LayerMask", "SeeThrough");

        if (Physics.Raycast(floorRay, out _, this.handUpMaxPosition.localPosition.y, mask)) {

            this.Catpass(closestPoint);

        }
        else {

            this.Hang(closestPoint);

        }




        this.RotateCharacterTowardsAttachedPoint();

        this.characterController.SetPerformingSoloJumpNRunMove(true);

        this.SetHandsOnLedge(this.attachedEdgePoint.position);


        this.attachedEdgePoint.parent = this.attachedGameObject.transform;
        this.attachedStandingPoint.parent = this.attachedGameObject.transform;
        this.characterController.transform.parent = this.attachedGameObject.transform;
    }


    private void SetHandsOnLedge(Vector3 closestPoint) {
        this.armRig.weight = 1;



        this.leftHandTarget.position = closestPoint;

        this.rightHandTarget.position = closestPoint;


        this.leftHandTarget.Translate(this.leftHandTarget.forward * this.handSpreadDistance);
        this.rightHandTarget.Translate(this.rightHandTarget.forward * this.handSpreadDistance);

    }

    private void FreeHandsFromLedge() {
        this.armRig.weight = 0;
    }

    private void RotateCharacterTowardsAttachedPoint() {
        this.characterController.physicalBody.transform.LookAt(new Vector3(this.attachedStandingPoint.position.x, this.characterController.transform.position.y, this.attachedStandingPoint.position.z));
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

        this.attachedStandingPoint.position = Vector3.zero;
        this.attachedEdgePoint.position = Vector3.zero;

        this.characterController.movementEnabled = true;



        this.attachedEdgePoint.parent = this.transform;
        this.attachedStandingPoint.parent = this.transform;


        if (this.attachedGameObject != null) {

            if (this.characterController.transform.parent == this.attachedGameObject.transform) {
                this.characterController.transform.parent = null;

                GameObject.DontDestroyOnLoad(this.characterController.gameObject);
            }
        }

        this.attachedGameObject = null;

        this.FreeHandsFromLedge();

    }



    private void Hang(Vector3 closestPoint) {

        this.isHanging = true;
        this.isCatpassing = false;
        this.isAttached = true;

        this.attachedStandingPoint.position = closestPoint;

        this.isUpjackingEnabled = false;

        this.characterController.animator.SetTrigger("Hang");
        this.audioSource.PlayOneShot(this.hangClip);

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

        Vector3 hangPos = new Vector3(this.attachedEdgePoint.position.x, (this.attachedStandingPoint.position - this.handUpMaxPosition.localPosition).y, this.attachedEdgePoint.position.z);

        Vector3 hangPosToCharXZDirection = this.characterController.transform.position - hangPos;
        hangPosToCharXZDirection.y = 0;

        hangPos += hangPosToCharXZDirection.normalized * this.hangEdgeDistance;

        this.characterController.transform.position = Vector3.Lerp(characterController.transform.position, hangPos, this.hangSlerpFactor);


    }

    private void Catpass(Vector3 closestPoint) {

        this.isHanging = false;
        this.isCatpassing = true;
        this.isAttached = true;

        this.attachedStandingPoint.position = closestPoint;


        this.isUpjackingEnabled = true;

        this.characterController.rb.isKinematic = true;


        this.characterController.animator.SetTrigger("Catpass");
    }

    private void CheckCharacterInputTowardsLedge() {


        if (this.IsCharacterInputTowardsLedge(this.attachedStandingPoint.position)) {

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
        this.characterController.animator.SetTrigger("HangDropDown");
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

    public void SetJackUpCompleted() {

        this.characterController.animator.ResetTrigger("HangLiftUp");
        this.characterController.animator.ResetTrigger("Catpass");

        this.characterController.SetPerformingSoloJumpNRunMove(false);



        this.DiscardLedge();
    }

    private void LiftUpCatpass() {
        Vector3 characterArrivedPosition = new Vector3(this.characterController.transform.position.x, this.attachedStandingPoint.position.y + this.liftUpExtraThreshhold, this.characterController.transform.position.z);

        this.characterController.transform.position = Vector3.Slerp(this.characterController.transform.position, characterArrivedPosition, this.catpassSlerpFactor);
    }


    private void LiftUpHang() {
        Vector3 characterArrivedPosition = new Vector3(this.characterController.transform.position.x, this.attachedStandingPoint.position.y + this.liftUpExtraThreshhold, this.characterController.transform.position.z);

        this.characterController.transform.position = Vector3.Slerp(this.characterController.transform.position, characterArrivedPosition, this.hangSlerpFactor);
    }



    private void LiftPlanar() {
        Vector3 characterArrivedPosition = this.attachedStandingPoint.position;


        this.characterController.transform.position = Vector3.Lerp(this.characterController.transform.position, characterArrivedPosition, this.catpassSlerpFactor);
    }

    private void FixedUpdate() {

        this.ControlHang();

        if (!this.isAttached) {
            return;
        }

        this.SetHandsOnLedge(this.attachedEdgePoint.position);

        MAFanSpinner fanSpinner = this.attachedGameObject.GetComponentInParent<MAFanSpinner>();
        if (fanSpinner != null) {
            fanSpinner.transportsCharacter = true;
            fanSpinner.transportedCharacter = this.characterController;
        }

        if (!this.isUpjackingEnabled) {
            return;
        }



        if (this.attachedStandingPoint.position == Vector3.zero) {
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


        if (this.attachedStandingPoint.position != Vector3.zero) {

            if (this.characterController.transform.position.y >= this.attachedStandingPoint.position.y) {
                this.SetHeightReached();
            }

        }

        Vector3 XZDistanceToClosestPointVector = this.attachedStandingPoint.position - new Vector3(this.characterController.transform.position.x, this.attachedStandingPoint.position.y, this.characterController.transform.position.z);
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
