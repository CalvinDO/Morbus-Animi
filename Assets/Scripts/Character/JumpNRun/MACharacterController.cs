using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public enum MACameraMode {
    ThirdPerson = 0,
    Fixed = 1
}

public enum MAKeyboardControlMode {
    Global = 0,
    CameraGoalAligned = 1,
    CameraCurrentAligned = 2,
    CameraGoalRotatorAligned = 3
}

public enum MACameraDirection {
    Back = 0,
    Right = 1,
    Front = 2,
    Left = 3
}

public enum MARotationMode {
    FirstPersonIdentical = 0,
    MovementDirectional = 1
}


public enum MASpaceType {
    Euclidean = 0,
    Radial = 1
}

public enum TransitionParameter {
    Move,
    Jump,
    Grounded,
    TransitionIndex
}

public class MACharacterController : MonoBehaviour {
    //[Range(0, 1000f)]
    //public float movementAcceleration;

    [Range(0, 10f)]
    public float transitionSpeed;
    [Range(0, 10f)]
    public float slideTransitionSpeed;
    [Range(0, 10f)]
    public float airTransitionSpeed;

    [Range(0, 10f)]
    public float maxMovementSpeed;

    [Range(1, 10f)]
    public float maxMovementSprintSpeedFactor;

    [Range(0, 550)]
    public float mouseSensitivity;

    public bool lockMouse = true;


    private bool directionInputExists = false;
    private bool isTooFast = false;

    public MAGroundCheck groundCheck;

    public Transform defaultTranslation;
    public Transform slideTransform;


    private bool isCollidingWall = false;
    private GameObject lastCollidedWall;
    private GameObject lastJumpedWall;
    private Vector3 wallJumpContactNormal;
    private bool lastJumpWasWalkWalk = false;

    [Range(0, 0.5f)]
    public float minWallJumpHeightAboveGround;
    public float minWalljumpVelocity = 0.5f;
    public float minWalljumpYVelocity = 0.5f;
    [Range(0, 100)]
    public float wallJumpImpulse;

    [Range(0, 1000)]
    public float jumpForce;

    [Range(0, 90)]
    public float maxRotationAngle;

    [Range(0, 0.2f)]
    public float slowDownFactor;

    public Rigidbody rb;

    [HideInInspector]
    public Vector3 speedAverage;

    public Vector3[] last3Speeds;

    private Vector3 speedBeforeWallContact;

    public GameObject xRotator;
    public GameObject yRotator;

    public GameObject cameraRotator;

    public GameObject physicalBody;

    float xRotationAmount = 0;
    float yRotationAmount = 0;


    float currentXRotation = 0;



    public Camera fpsCamera;
    public Camera mainCamera;

    private Transform currentCameraGoal;
    public Transform fixedCameraGoal;
    public Transform thirdPCameraGoal;
    public Transform goalRotator;


    public MACameraMode cameraMode = MACameraMode.ThirdPerson;
    public MAKeyboardControlMode keyboardControlMode = MAKeyboardControlMode.Global;
    public MARotationMode rotationMode = MARotationMode.MovementDirectional;

    [Range(0, 1)]
    public float camSlerpFactor;


    private bool isSprinting = false;
    private float timeSinceSprintStarted = 0;
    public float slideSpeedThreshhold;
    public float SprintFOVLerpFactor;

    [HideInInspector]
    public bool movementEnabled = true;
    int framesTillStart = 0;

    int framesSinceJump = 0;
    bool inJump = false;
    private bool isGrounded;

    float threashold = 0.001f;

    float millisecondsSinceStart = 0;



    public float bobbingYIntensity = 0.5f;
    public float bobbingXIntensity = 0.25f;

    public float bobbingSpeed = 0.5f;


    public GameObject flashlight;


    public float flashlightXIntensity = 0.001f;
    public float flashlightYIntensity = 0.001f;

    public float flashlightBobbingYSpeed = 0.5f;
    public float flashlightBobbingXSpeed = 0.5f;

    float scaledTimeSinceStart = 0;

    public bool isBobbingEnabled = false;
    public float standardFOV;
    public float sprintFOV;


    public Animator animator;
    public float idleVelocityThreshhold;

    //for item interaction
    //MAInteractable hover;
    //MASprayable wall;
    //public GameObject playerInventory;


    public MASpaceType spaceType;

    public AudioSource footsteps;

    public float maxSlideDuration;
    private float remainingSlideTime;
    private bool isSliding = false;

    public Collider defaultCollider;
    public Collider slideCollider;

    public GameObject slideTransforms;

    public MACharacterSwingTrigger characterSwingTrigger;
    public Transform swingGrabPosition;
    public Transform swingFootUpPosition;

    [Range(0, 3)]
    public float swingReleaseVelocityFactor;

    private bool isSwinging = false;

    public MALedgeInteraction ledgeInteraction;

    public bool jumped;
    public bool up;
    public bool down;
    public bool left;
    public bool right;

    void Start() {

        this.currentXRotation = 0;
        this.xRotationAmount = 0;


        Cursor.lockState = this.lockMouse ? CursorLockMode.Locked : CursorLockMode.None;

        this.last3Speeds = new Vector3[3];
        this.speedBeforeWallContact = Vector3.zero;

        this.remainingSlideTime = this.maxSlideDuration;


        this.mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();

        // this.rb.velocity = new Vector3(0, 5, 3);
    }
    void Update() {

        this.CalculateMovement();

        this.CalculateSpeedAverage();

        this.ControlAnimationAndVelocityRotation();
        this.ManageUserControlledCamGoalRotation();
        this.ManageJumpNRun();
        //this.ManageRaycastInteraction();



        this.framesTillStart++;
        this.millisecondsSinceStart += Time.deltaTime;
        this.scaledTimeSinceStart += Time.deltaTime;
    }

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
        ledgeInteraction = this.GetComponentInChildren<MALedgeInteraction>();
    }
    private void OnApplicationPause(bool pause) {
        //currently dead code
        Cursor.lockState = CursorLockMode.None;
    }

    private void FixedUpdate() {

        //this.CalculateAimRotation();

        if (this.inJump) {
            this.framesSinceJump++;
        }

        this.SlowDown();

        this.ManageSmartCam();
    }

    private void CalculateMovement() {

        if (!this.isGrounded) {
            //return;
        }

        this.AccelerateXZ();

        //this.LimitSpeed();


        if (this.isBobbingEnabled) {
            this.CalculateBob();
        }
    }


    private void CalculateSpeedAverage() {
        this.last3Speeds[2] = this.last3Speeds[1];
        this.last3Speeds[1] = this.last3Speeds[0];
        this.last3Speeds[0] = this.rb.velocity;

        Vector3 sum = Vector3.zero;
        for (int index = 0; index < this.last3Speeds.Length; index++) {
            sum += this.last3Speeds[index];
        }

        this.speedAverage = sum / this.last3Speeds.Length;
    }


    private void ControlAnimationAndVelocityRotation() {

        if (this.rb.velocity.magnitude > this.idleVelocityThreshhold) {
            this.physicalBody.transform.rotation = this.slideTransforms.transform.rotation = Quaternion.Euler(0, Vector3.SignedAngle(Vector3.forward, Vector3.ProjectOnPlane(this.rb.velocity, Vector3.up), Vector3.up), 0);

            if (this.isSliding) {
                this.physicalBody.transform.rotation = this.slideTransform.rotation;
            }
        }

        if (this.isSprinting) {
            this.animator.SetBool("isSprinting", true);
            this.animator.SetBool("isWalking", false);
            return;
        }

        this.animator.SetBool("isSprinting", false);


        if (Vector3.ProjectOnPlane(this.rb.velocity, Vector3.up).magnitude > this.idleVelocityThreshhold) {
            this.animator.SetBool("isWalking", true);
            return;
        }

        this.animator.SetBool("isWalking", false);
    }


    private void ManageSmartCam() {

        switch (this.cameraMode) {
            case MACameraMode.ThirdPerson:
                this.currentCameraGoal = this.thirdPCameraGoal;
                break;
            case MACameraMode.Fixed:
                this.currentCameraGoal = this.fixedCameraGoal;
                break;
            default:
                this.currentCameraGoal = this.thirdPCameraGoal;
                break;
        }

        this.SlerpCam();
    }


    private void ManageUserControlledCamGoalRotation() {
        if (this.spaceType == MASpaceType.Radial) {
            //this.ManageUserControlledRadialCamGoalRotation();
            //return;
        }

        if (Input.GetKeyUp(KeyCode.RightArrow)) {
            this.goalRotator.Rotate(Vector3.up * 90);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow)) {
            this.goalRotator.Rotate(Vector3.up * -90);
        }

    }

    private void ManageUserControlledRadialCamGoalRotation() {
        float phi = Vector3.Angle(this.transform.position, Vector3.forward);
        this.goalRotator.rotation = Quaternion.Euler(0, phi, 0);
    }

    public void SetDefaultModeAndSpaceType() {
        this.cameraMode = MACameraMode.ThirdPerson;
        this.spaceType = MASpaceType.Radial;
        this.keyboardControlMode = MAKeyboardControlMode.CameraGoalRotatorAligned;
        this.fixedCameraGoal = null;
    }

    private void SlerpCam() {
        switch (this.cameraMode) {
            case MACameraMode.Fixed:

                this.mainCamera.transform.position = Vector3.Slerp(this.mainCamera.transform.position, this.currentCameraGoal.position, this.camSlerpFactor);
                this.mainCamera.transform.rotation = Quaternion.Slerp(this.mainCamera.transform.rotation, this.currentCameraGoal.rotation, this.camSlerpFactor);

                break;
            case MACameraMode.ThirdPerson:
                this.mainCamera.transform.SetPositionAndRotation(this.currentCameraGoal.position, this.currentCameraGoal.rotation);
                break;
        }
    }


    private void OnCollisionStay(Collision collision) {

        if (collision.gameObject.CompareTag("JumpNRunElement")) {

            if (this.isGrounded) {
                this.isCollidingWall = false;
                this.lastCollidedWall = null;
                return;
            }

            this.isCollidingWall = true;
            this.lastCollidedWall = collision.gameObject;

            if (collision.contactCount > 1) {
                if (collision.GetContact(0).normal != collision.GetContact(1).normal) {
                    Debug.LogWarning("collision contact 0 != collision contat 1 !");
                }
            }

            this.wallJumpContactNormal = collision.GetContact(0).normal;
        }

        //MAInteractable interactable = collision.collider.GetComponent<MAInteractable>();
        //if (interactable != null) {
        //    this.hover = interactable;
        //    this.hover.setHover();
        //}
        //if (Input.GetMouseButtonDown(1)) {
        //    MASprayable sprayable = collision.collider.GetComponent<MASprayable>();
        //    if (sprayable != null) {
        //        this.wall = sprayable;
        //        //Vector3 difference = this.transform.position - hit.point;                    //have to change spray since interaction is now collider based instead of raycast
        //        //this.wall.Spray(hit.point, sprayable.transform.rotation, difference);
        //    }
        //    else {
        //        this.wall = null;
        //    }
        //}
        //if (Input.GetKey("e") && interactable != null) {
        //    this.hover.MAInteract();
        //    //this.hover.clearText();
        //}
    }

    private void OnCollisionExit(Collision collision) {

        if (collision.gameObject.CompareTag("JumpNRunElement")) {

            this.isCollidingWall = false;
        }

        //    if (this.hover != null) {
        //        this.hover.removeHover();
        //        this.hover = null;
        //    }
    }

    private void OnCollisionEnter(Collision collision) {

        this.rb.velocity = new Vector3(this.last3Speeds[0].x, this.rb.velocity.y, this.last3Speeds[0].z);

        if (collision.gameObject.CompareTag("JumpNRunElement")) {

            this.speedBeforeWallContact = this.rb.velocity;
        }
    }


    private void AccelerateXZ() {

        if (!this.movementEnabled) {
            return;
        }

        this.directionInputExists = false;

        Vector3 normalizedSummedInput = this.getNormalizedSummedInputVector();


        this.ManageSprinting();

        if (this.directionInputExists) {

            this.playFootsteps();

            /*
            Vector3 scaledNormalizedResult = normalizedSummedInput * this.movementAcceleration;

            if (!this.isTooFast) {
                this.rb.AddForce(scaledNormalizedResult, ForceMode.Acceleration);
            }
            */

            Vector3 desiredVelocity;




            if (this.isSprinting) {
                desiredVelocity = normalizedSummedInput * this.maxMovementSpeed * this.maxMovementSprintSpeedFactor;
            }
            else {
                desiredVelocity = normalizedSummedInput * this.maxMovementSpeed;
            }




            Vector3 oldVelocity = Vector3.ProjectOnPlane(this.rb.velocity, Vector3.up);


            float currentTransitionSpeed = this.isGrounded ? (this.isSliding ? this.slideTransitionSpeed : this.transitionSpeed) : this.airTransitionSpeed;


            Vector3 newVelocity = oldVelocity * (1 - Time.deltaTime * currentTransitionSpeed) + desiredVelocity * (Time.deltaTime * currentTransitionSpeed);

            newVelocity += Vector3.up * this.rb.velocity.y;

            this.rb.velocity = newVelocity;
        }
    }


    private void ManageSprinting() {
        if (Input.GetKey("left shift") && this.directionInputExists) {
            this.isSprinting = true;
            //this.mainCamera.fieldOfView = Mathf.Lerp(this.standardFOV, this.sprintFOV, this.timeSinceSprintStarted);

            this.timeSinceSprintStarted += this.SprintFOVLerpFactor;
        }
        else {
            this.isSprinting = false;
            //this.mainCamera.fieldOfView = Mathf.Lerp(this.sprintFOV, this.standardFOV, this.timeSinceSprintStarted);

            this.timeSinceSprintStarted -= this.SprintFOVLerpFactor;
        }

        this.timeSinceSprintStarted = Mathf.Clamp(this.timeSinceSprintStarted, 0, 1);
    }


    private Vector3 getNormalizedSummedInputVector() {
        Vector3 resultingVector = Vector3.zero;

        resultingVector += this.GetUnitVectorToInput(KeyCode.W);
        resultingVector += this.GetUnitVectorToInput(KeyCode.A);
        resultingVector += this.GetUnitVectorToInput(KeyCode.S);
        resultingVector += this.GetUnitVectorToInput(KeyCode.D);

        return resultingVector.normalized;
    }

    private Vector3 GetUnitVectorToInput(KeyCode keyCode) {

        if (!Input.GetKey(keyCode)) {
            return Vector3.zero;
        }

        Vector3 direction = Vector3.zero;

        switch (keyCode) {
            case KeyCode.W:
                direction = Vector3.forward;
                up = true;
                left = down = right = false;
                break;
            case KeyCode.A:
                direction = Vector3.left;
                left = true;
                right = down = up = false;
                break;
            case KeyCode.S:
                direction = Vector3.back;
                down = true;
                left = right = up = false;
                break;
            case KeyCode.D:
                direction = Vector3.right;
                right = true;
                left = up = down = false;
                break;
            default:
                return Vector3.zero;
        }

        Vector3 directionAccordingToInput = GetUnitVectorInInputDirection(direction);

        this.directionInputExists = true;
        return directionAccordingToInput;
    }

    private void playFootsteps() {
        if (this.footsteps.isPlaying) {
            return;
        }
        this.footsteps.PlayOneShot(this.footsteps.clip);
    }



    private Vector3 GetUnitVectorInInputDirection(Vector3 direction) {

        if (this.spaceType == MASpaceType.Radial) {
            return this.GetRadialMovementVectorInDirection(direction);
        }

        Vector3 rotated;

        switch (this.keyboardControlMode) {
            case MAKeyboardControlMode.CameraGoalAligned:
                rotated = this.currentCameraGoal.transform.rotation * direction;
                break;
            case MAKeyboardControlMode.CameraGoalRotatorAligned:
                rotated = this.goalRotator.transform.localRotation * direction;
                break;
            case MAKeyboardControlMode.CameraCurrentAligned:
                rotated = this.mainCamera.transform.rotation * direction;
                break;
            case MAKeyboardControlMode.Global:
                rotated = this.transform.rotation * direction;
                break;
            default:
                rotated = this.transform.rotation * direction;
                break;
        }

        return Vector3.ProjectOnPlane(rotated, Vector3.up).normalized;
    }

    private Vector3 GetRadialMovementVectorInDirection(Vector3 direction) {
        Vector3 rotated = Vector3.zero;

        if (direction.z == 1) {
            rotated = -this.transform.position;
            //return Vector3.ProjectOnPlane(-this.transform.position, Vector3.up).normalized;
        }

        if (direction.z == -1) {
            rotated = this.transform.position;
            //return Vector3.ProjectOnPlane(this.transform.position, Vector3.up).normalized;
        }

        if (direction.x == 1) {
            rotated = Quaternion.Euler(0, -90, 0) * this.transform.position;
            // return Vector3.ProjectOnPlane(Quaternion.Euler(0, -90, 0) * this.transform.position, Vector3.up).normalized;
        }

        if (direction.x == -1) {
            rotated = Quaternion.Euler(0, 90, 0) * this.transform.position;
            //return Vector3.ProjectOnPlane(Quaternion.Euler(0, 90, 0) * this.transform.position, Vector3.up).normalized;
        }

        switch (this.keyboardControlMode) {
            case MAKeyboardControlMode.CameraGoalAligned:
                throw new Exception("Radial Space + CameraGoalAligned not implemented!");
                break;
            case MAKeyboardControlMode.CameraCurrentAligned:
                throw new Exception("Radial Space + CameraCurrentAligned not implemented!");
                break;
            case MAKeyboardControlMode.Global:
                break;
            case MAKeyboardControlMode.CameraGoalRotatorAligned:
                rotated = this.goalRotator.transform.localRotation * rotated;
                break;
            default:
                break;
        }

        return Vector3.ProjectOnPlane(rotated, Vector3.up).normalized;
    }


    [Obsolete("Limiting Speed is no longer needed because Character Movement is velocity-controlled, not acceleration-physics-controlled")]
    private void LimitSpeed() {
        //Limit the Player Speed because without it acceleration would result in infinite speed!

        if (!this.directionInputExists) {
            return;
        }


        Vector3 velocityXZ = Vector3.ProjectOnPlane(this.rb.velocity, Vector3.up);

        this.isTooFast = false;
        float maxSpeedForCompare;

        float maxMovementSprintSpeed = this.maxMovementSprintSpeedFactor * this.maxMovementSpeed;


        switch (this.isSprinting) {

            case true:

                this.isTooFast = velocityXZ.magnitude > maxMovementSprintSpeed;
                maxSpeedForCompare = this.maxMovementSprintSpeedFactor;

                break;
            case false:

                this.isTooFast = velocityXZ.magnitude > this.maxMovementSpeed;
                maxSpeedForCompare = this.maxMovementSpeed;

                break;
        }

        //Depricated code to set the character speed to the max speed.
        //Depricated because it produces continous walking when changing directions while pressing other keys
        /*
        if (this.isTooFast) {

            Vector3 newVelocityXZ = velocityXZ.normalized * maxSpeedForCompare * 0.94f;

            this.rb.velocity = new Vector3(newVelocityXZ.x, this.rb.velocity.y, newVelocityXZ.z);
        }
        */
    }


    private void SlowDown() {

        if (!this.directionInputExists && this.isGrounded) {
            Vector3 velocity = this.rb.velocity;

            velocity.x *= (1 - this.slowDownFactor);
            velocity.z *= (1 - this.slowDownFactor);

            this.rb.velocity = velocity;
        }
    }


    private void ManageJumpNRun() {
        this.ManageJump();
        this.ManageSlide();
        this.ManageSwing();
    }

    private void ManageSwing() {
        if (this.isGrounded) {
            return;
        }

        if (!this.characterSwingTrigger.isSwingbarReachable) {
            return;
        }

        if (this.isSwinging) {
            if (!Input.GetKey(KeyCode.Space)) {
                //Stop Swinging
                this.isSwinging = false;
                this.characterSwingTrigger.reachableSwingbar.ReleaseCharacter(this);
            }
            else {
                return;
            }
        }

        if (Input.GetKey(KeyCode.Space)) {
            this.characterSwingTrigger.reachableSwingbar.SetCharacterInitialPosition(this);
            this.isSwinging = true;
        }
    }

    private void ManageSlide() {

        if (this.isSliding) {
            this.remainingSlideTime -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.C)) {

            if (this.isGrounded) {

                if (this.isSprinting) {

                    Vector3 currentXZvelocity = Vector3.ProjectOnPlane(this.rb.velocity, Vector3.up);
                    Vector2 currentXZvelocity2D = new Vector2(currentXZvelocity.x, currentXZvelocity.z);

                    if (currentXZvelocity2D.magnitude > this.slideSpeedThreshhold) {
                        this.StartSliding();
                    }
                }
            }
        }

        if (this.remainingSlideTime <= 0) {
            this.EndSliding();
            return;
        }

        if (Input.GetKeyUp(KeyCode.C)) {
            this.EndSliding();
        }
    }

    private void StartSliding() {
        this.physicalBody.transform.position = this.slideTransform.position;

        this.defaultCollider.gameObject.SetActive(false);
        this.slideCollider.gameObject.SetActive(true);

        this.isSliding = true;

        this.movementEnabled = false;
    }

    private void EndSliding() {
        this.physicalBody.transform.localPosition = Vector3.zero;
        this.physicalBody.transform.rotation = Quaternion.identity;
        this.remainingSlideTime = this.maxSlideDuration;


        this.defaultCollider.gameObject.SetActive(true);
        this.slideCollider.gameObject.SetActive(false);

        this.isSliding = false;
        this.movementEnabled = true;
    }

    private void ManageJump() {

        if (Input.GetKey("space")) {
            jumped = true;

            if (Input.GetKeyDown("space")) {

                if (this.isGrounded) {
                    this.PerformSimpleJump();
                    this.lastJumpWasWalkWalk = false;
                }
                else {
                    if (this.IsWallJumpOrWalkAllowed()) {
                        this.PerformWallJump();
                        this.lastJumpWasWalkWalk = false;
                    }
                }
            }
            else {
                if (!this.isGrounded) {
                    if (this.IsWallJumpOrWalkAllowed()) {
                        if (!this.lastJumpWasWalkWalk) {
                            this.PerformWallWalk();
                        }
                    }
                }
            }
        }
        else jumped = false;
    }

    private bool IsWallJumpOrWalkAllowed() {

        Vector3 XZPlaneProjectedSpeedBeforeWall = Vector3.ProjectOnPlane(this.speedBeforeWallContact, Vector3.up);
        Vector2 XZSpeedBeforeWall = new Vector2(XZPlaneProjectedSpeedBeforeWall.x, XZPlaneProjectedSpeedBeforeWall.z);
        float ySpeedBeforeWall = this.speedBeforeWallContact.y;

        bool isXZMagnitudeHighEnough = XZSpeedBeforeWall.magnitude > this.minWalljumpVelocity;
        bool isYSpeedHighEnough = ySpeedBeforeWall > this.minWalljumpYVelocity;

        bool wallPreviouslyJumped = this.lastJumpedWall == this.lastCollidedWall;

        bool highEnoughAboveGround = !Physics.Raycast(new Ray(this.transform.position, Vector3.down), out _, this.minWallJumpHeightAboveGround);

        return this.isCollidingWall && isXZMagnitudeHighEnough && isYSpeedHighEnough && !wallPreviouslyJumped && highEnoughAboveGround;
    }

    private void PerformWallJump() {

        this.rb.AddForce(this.wallJumpContactNormal * this.wallJumpImpulse, ForceMode.Impulse);

        this.PerformSimpleJump();

        this.SetLastJumpedWall();

    }

    private void PerformWallWalk() {
        this.PerformSimpleJump();
        this.SetLastJumpedWall();
        this.lastJumpWasWalkWalk = true;
    }

    private void SetLastJumpedWall() {
        this.lastJumpedWall = this.lastCollidedWall;
    }


    private void PerformSimpleJump() {
        Vector3 force = Vector3.up * this.jumpForce;

        rb.AddForce(force, ForceMode.Impulse);

        this.transform.Translate(Vector3.up * 0.01f);

        this.inJump = true;
    }

    private void CalculateBob() {

        Vector3 velocityXZ = this.speedAverage;
        velocityXZ.y = 0;

        float magnitude = velocityXZ.magnitude;

        if (Mathf.Abs(velocityXZ.magnitude) <= this.threashold) {
            return;
        }

        float oszillatedBobX = Mathf.Cos(this.scaledTimeSinceStart * magnitude * this.bobbingSpeed * (2 * Mathf.PI) * 2) * this.bobbingXIntensity;
        float oszillatedBobY = Mathf.Sin(this.scaledTimeSinceStart * magnitude * this.bobbingSpeed * (2 * Mathf.PI)) * this.bobbingYIntensity;
        //this.camera.transform.Translate(new Vector3(oszillatedBobY, oszillatedBobX, 0));

        Vector3 oscillatedBobVector = new Vector3(oszillatedBobY, oszillatedBobX, 0);
        oscillatedBobVector = this.fpsCamera.transform.rotation * oscillatedBobVector;


        this.cameraRotator.transform.position += oscillatedBobVector;


        float yOszillation = Mathf.Cos(this.scaledTimeSinceStart * magnitude * this.flashlightBobbingYSpeed * (2 * Mathf.PI) * 2) * this.flashlightYIntensity;
        float xOszillation = Mathf.Sin(this.scaledTimeSinceStart * magnitude * this.flashlightBobbingXSpeed * (2 * Mathf.PI)) * this.flashlightXIntensity;


        // this.flashlight.transform.Translate(new Vector3(xOszillation, yOszillation, 0));
    }


    [Obsolete("CalculateAimRotation is deprecated, affected Transforms get rotated by the MouseOrbit - Rotation automatically")]
    private void CalculateAimRotation() {
        if (this.framesTillStart > 5) {
            switch (this.rotationMode) {
                case MARotationMode.FirstPersonIdentical:
                    this.CalculateFirstPersonRotation();
                    break;
                case MARotationMode.MovementDirectional:
                    this.CalculateMovementDirectionalRotation();
                    break;
                default:
                    break;
            }
        }
    }

    private void CalculateMovementDirectionalRotation() {
        if (this.spaceType == MASpaceType.Radial) {
            this.CalculateRadialMovementDirectionalRotation();
            return;
        }

        //this.yRotator.transform.rotation = Quaternion.identity;
        // this.yRotator.transform.rotation = Quaternion.Euler(0, 0, 4);
    }

    private void CalculateRadialMovementDirectionalRotation() {
        float phi = Vector3.Angle(Vector3.ProjectOnPlane(this.transform.position, Vector3.up), Vector3.back);

        if (this.transform.position.x < 0) {
            phi = 360 - phi;
        }

        this.transform.rotation = Quaternion.Euler(0, -phi, 0);
    }

    private void CalculateFirstPersonRotation() {
        this.xRotationAmount = -Input.GetAxis("Mouse Y") * this.mouseSensitivity * Time.deltaTime;
        this.currentXRotation += this.xRotationAmount;

        this.yRotationAmount = Input.GetAxis("Mouse X") * this.mouseSensitivity * Time.deltaTime;

        this.yRotator.transform.Rotate(Vector3.up, this.yRotationAmount);


        if (this.currentXRotation > this.maxRotationAngle) {
            this.currentXRotation = this.maxRotationAngle;
        }
        else if (this.currentXRotation < -this.maxRotationAngle) {
            this.currentXRotation = -this.maxRotationAngle;
        }
        else {
            this.xRotator.transform.Rotate(Vector3.right, this.xRotationAmount);
        }
    }

    public void Ground() {
        this.isGrounded = true;

        if (this.jumped) {
            return;
        }
        this.lastCollidedWall = null;
        this.lastJumpedWall = null;
    }

    public void DeGround() {
        this.isGrounded = false;
    }
}

