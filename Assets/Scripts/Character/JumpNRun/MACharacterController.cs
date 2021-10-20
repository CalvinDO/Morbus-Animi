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



public class MACharacterController : MonoBehaviour {
    [Range(0, 1000f)]
    public float movementAcceleration;

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

    public float minWalljumpVelocity = 0.5f;
    public float minWalljumpYVelocity = 0.5f;

    [Range(0, 1000)]
    public float jumpForce;

    [Range(0, 90)]
    public float maxRotationAngle;

    [Range(0, 0.2f)]
    public float slowDownFactor;

    public Rigidbody rb;

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


    private bool sprinting = false;
    private float timeSinceSprintStarted = 0;
    public float slideSpeedThreshhold;
    public float SprintFOVLerpFactor;

    bool movementEnabled = true;
    int framesTillStart = 0;

    int framesTillJump = 0;
    bool inJump = false;

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
    MAInteractable hover;
    MASprayable wall;
    //public GameObject playerInventory;


    public MASpaceType spaceType;

    public AudioSource footsteps;

    public float maxSlideDuration;
    private float remainingSlideTime;
    private bool isSliding = false;

    public Collider defaultCollider;
    public Collider slideCollider;

    void Start() {

        this.currentXRotation = 0;
        this.xRotationAmount = 0;


        //Cursor.lockState = this.lockMouse? CursorLockMode.Locked: CursorLockMode.None;

        this.last3Speeds = new Vector3[3];
        this.speedBeforeWallContact = Vector3.zero;

        this.remainingSlideTime = this.maxSlideDuration;

        this.rb.velocity = new Vector3(0, 5, 3);
    }


    void Update() {

        this.ManageUserControlledCamGoalRotation();
        this.ManageJumpNRun();
        this.ManageInteraction();


        this.ControlAnimation();

        this.framesTillStart++;
        this.millisecondsSinceStart += Time.deltaTime;
        this.scaledTimeSinceStart += Time.deltaTime;


    }

    private void FixedUpdate() {

        this.CalculateMovement();

        this.CalculateRotation();


        if (this.inJump) {
            this.framesTillJump++;
        }
        this.CalculateSpeedAverage();

        this.ManageSmartCam();

    }

    private void CalculateMovement() {

        if (!this.groundCheck.isGrounded) {
            return;
        }


        this.AccelerateXZ();

        this.LimitSpeed();

        //this.SlowDown();

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


    private void ControlAnimation() {
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
        this.mainCamera.transform.position = Vector3.Slerp(this.mainCamera.transform.position, this.currentCameraGoal.position, this.camSlerpFactor);
        this.mainCamera.transform.rotation = Quaternion.Slerp(this.mainCamera.transform.rotation, this.currentCameraGoal.rotation, this.camSlerpFactor);
    }


    private void OnCollisionStay(Collision collision) {

        if (collision.gameObject.CompareTag("JumpNRunElement")) {

            this.isCollidingWall = true;
        }

    }

    private void OnCollisionExit(Collision collision) {

        if (collision.gameObject.CompareTag("JumpNRunElement")) {

            this.isCollidingWall = false;
        }
    }

    private void OnCollisionEnter(Collision collision) {

        this.rb.velocity = new Vector3(this.last3Speeds[0].x, this.rb.velocity.y, this.last3Speeds[0].z);

        if (collision.gameObject.CompareTag("JumpNRunElement")) {

            this.speedBeforeWallContact = this.rb.velocity;
            this.isCollidingWall = true;
        }
    }


    private void AccelerateXZ() {

        if (!this.movementEnabled) {
            return;
        }

        this.directionInputExists = false;

        Vector3 normalizedSummedInput = this.getNormalizedSummedInputVector();


        if (this.directionInputExists) {

            this.playFootsteps();
            this.ManageSprinting();

            Vector3 scaledNormalizedResult = normalizedSummedInput * this.movementAcceleration;

            if (!this.isTooFast) {
                this.rb.AddForce(scaledNormalizedResult, ForceMode.Acceleration);
            }
        }
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
                break;
            case KeyCode.A:
                direction = Vector3.left;
                break;
            case KeyCode.S:
                direction = Vector3.back;
                break;
            case KeyCode.D:
                direction = Vector3.right;
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


    private void ManageSprinting() {
        if (Input.GetKey("left shift")) {
            this.sprinting = true;
            //this.mainCamera.fieldOfView = Mathf.Lerp(this.standardFOV, this.sprintFOV, this.timeSinceSprintStarted);

            this.timeSinceSprintStarted += this.SprintFOVLerpFactor;
        }
        else {
            this.sprinting = false;
            //this.mainCamera.fieldOfView = Mathf.Lerp(this.sprintFOV, this.standardFOV, this.timeSinceSprintStarted);

            this.timeSinceSprintStarted -= this.SprintFOVLerpFactor;
        }

        this.timeSinceSprintStarted = Mathf.Clamp(this.timeSinceSprintStarted, 0, 1);
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

    private void LimitSpeed() {
        //Limit the Player Speed because without it acceleration would result in infinite speed!

        if (!this.directionInputExists) {
            return;
        }


        Vector3 velocityXZ = Vector3.ProjectOnPlane(this.rb.velocity, Vector3.up);

        this.isTooFast = false;
        float maxSpeedForCompare;

        float maxMovementSprintSpeed = this.maxMovementSprintSpeedFactor * this.maxMovementSpeed;


        switch (this.sprinting) {

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

        if (!(Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")) && this.groundCheck.isGrounded) {
            Vector3 velocity = this.rb.velocity;

            velocity.x *= (1 - this.slowDownFactor);
            velocity.z *= (1 - this.slowDownFactor);

            this.rb.velocity = velocity;
        }
    }


    private void ManageJumpNRun() {
        this.ManageJump();
        this.ManageSlide();
    }

    private void ManageSlide() {

        if (this.isSliding) {
            this.remainingSlideTime -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) {


            if (this.groundCheck.isGrounded) {
                Vector3 currentXZvelocity = Vector3.ProjectOnPlane(this.rb.velocity, Vector3.up);
                Vector2 currentXZvelocity2D = new Vector2(currentXZvelocity.x, currentXZvelocity.z);

                if (currentXZvelocity2D.magnitude > this.slideSpeedThreshhold) {
                    this.StartSliding();
                }
            }
        }

        if (this.remainingSlideTime <= 0) {
            this.EndSliding();
            return;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl)) {
            this.EndSliding();
        }
    }

    private void StartSliding() {
        this.physicalBody.transform.position = this.slideTransform.position;
        this.physicalBody.transform.rotation = this.slideTransform.rotation;

        this.defaultCollider.gameObject.SetActive(false);
        this.slideCollider.gameObject.SetActive(true);

        this.isSliding = true;
    }

    private void EndSliding() {
        this.physicalBody.transform.localPosition = Vector3.zero;
        this.physicalBody.transform.rotation = Quaternion.identity;
        this.remainingSlideTime = this.maxSlideDuration;


        this.defaultCollider.gameObject.SetActive(true);
        this.slideCollider.gameObject.SetActive(false);

        this.isSliding = false;
    }

    private void ManageJump() {

        if (Input.GetKeyDown("space")) {

            if (this.groundCheck.isGrounded) {
                this.PerformeSimpleJump();
            }
            else {
                if (this.WallJumpAllowed()) {
                    this.PerformWallJump();
                }
            }
        }
    }

    private bool WallJumpAllowed() {

        Vector3 XZPlaneProjectedSpeedBeforeWall = Vector3.ProjectOnPlane(this.speedBeforeWallContact, Vector3.up);
        Vector2 XZSpeedBeforeWall = new Vector2(XZPlaneProjectedSpeedBeforeWall.x, XZPlaneProjectedSpeedBeforeWall.z);
        float ySpeedBeforeWall = this.speedBeforeWallContact.y;

        bool isXZMagnitudeHighEnough = XZSpeedBeforeWall.magnitude > this.minWalljumpVelocity;
        bool isYSpeedHighEnough = ySpeedBeforeWall > this.minWalljumpYVelocity;

        Debug.Log(XZSpeedBeforeWall.magnitude);

        return this.isCollidingWall && isXZMagnitudeHighEnough && isYSpeedHighEnough;
    }

    private void PerformWallJump() {

        this.rb.AddForce(this.getNormalizedSummedInputVector() * 200, ForceMode.Impulse);

        this.PerformeSimpleJump();

    }


    private void PerformeSimpleJump() {
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


    private void CalculateRotation() {
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

    private void ManageInteraction() {

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(1)) {
            if (Physics.Raycast(ray, out hit)) {
                MASprayable sprayable = hit.collider.GetComponent<MASprayable>();
                if (sprayable != null) {
                    this.wall = sprayable;
                    Vector3 difference = this.transform.position - hit.point;
                    this.wall.Spray(hit.point, sprayable.transform.rotation, difference);
                }
                else {
                    this.wall = null;
                }
            }
        }
        if (Input.GetMouseButtonDown(0)) {

            if (Physics.Raycast(ray, out hit)) {
                MAInteractable interactable = hit.collider.GetComponent<MAInteractable>();
                if (interactable != null) {
                    this.hover = interactable;
                    this.hover.setHover();
                }
                else {
                    if (this.hover != null) {
                        this.hover.removeHover();

                        this.hover = null;
                    }
                }
                if (this.hover == interactable) {
                    if (Input.GetMouseButtonDown(0) && interactable != null) {
                        this.hover.MAInteract();
                        //this.hover.clearText();
                    }
                }
            }
        }
    }
}
