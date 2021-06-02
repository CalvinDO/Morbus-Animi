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
    CameraCurrentAligned = 2
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





public class MACharacterController : MonoBehaviour {
    [Range(0, 1000f)]
    public float movementAcceleration;

    [Range(0, 10f)]
    public float maxMovementSpeed;

    [Range(1, 3f)]
    public float maxMovementSprintSpeedFactor;

    [Range(0, 550)]
    public float mouseSensitivity;

    [Range(0, 1000)]
    public float jumpForce;

    [Range(0, 90)]
    public float maxRotationAngle;

    [Range(0, 0.2f)]
    public float slowDownFactor;

    public Rigidbody rb;

    public Vector3 speedAverage;

    public Vector3[] last3Speeds;


    public GameObject xRotator;
    public GameObject yRotator;

    public GameObject cameraRotator;



    float xRotationAmount = 0;
    float yRotationAmount = 0;


    float currentXRotation = 0;



    public Camera fpsCamera;
    public Camera mainCamera;

    private Transform currentCameraGoal;
    public Transform fixedCameraGoal;
    public Transform thirdPCameraGoal;
    public Transform goalRotator;

    [HideInInspector]
    public MACameraMode cameraMode = MACameraMode.ThirdPerson;
    public MAKeyboardControlMode keyboardControlMode = MAKeyboardControlMode.Global;
    public MARotationMode rotationMode = MARotationMode.MovementDirectional;

    [Range(0, 1)]
    public float camSlerpFactor;


    bool isGrounded = true;

    private bool sprinting = false;
    private float timeSinceSprintStarted = 0;
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
    public Animation animation;


    void Start() {
        this.currentXRotation = 0;
        this.xRotationAmount = 0;

        Cursor.lockState = CursorLockMode.Locked;

        this.last3Speeds = new Vector3[3];
    }


    void Update() {
        this.ManageCamGoalRotation();
        this.ManageJump();
        this.ManageSmartCam();

        this.ManageIdleAnimation();

    }

    private void FixedUpdate() {
        this.CalculateMovement();

        this.CalculateRotation();


        if (this.inJump) {
            this.framesTillJump++;
        }
        this.CalculateSpeedAverage();


        this.framesTillStart++;
        this.millisecondsSinceStart += Time.deltaTime;
        this.scaledTimeSinceStart += Time.deltaTime;
    }

    private void CalculateMovement() {
        this.AccelerateXZ();

        this.LimitSpeed();

        this.SlowDown();


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


    private void ManageIdleAnimation() {
        if (this.rb.velocity.magnitude > 0.1f) {
            this.animator.Play("New Animation", -1, 1.3f);
        }
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


    private void ManageCamGoalRotation() {
        if (Input.GetKeyUp(KeyCode.RightArrow)) {
            this.goalRotator.Rotate(Vector3.up * 90);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow)) {
            this.goalRotator.Rotate(Vector3.up * 90);
        }
    }


    private void SlerpCam() {
        this.mainCamera.transform.position = Vector3.Slerp(this.mainCamera.transform.position, this.currentCameraGoal.position, this.camSlerpFactor);
        this.mainCamera.transform.rotation = Quaternion.Slerp(this.mainCamera.transform.rotation, this.currentCameraGoal.rotation, this.camSlerpFactor);
    }


    private void OnCollisionStay(Collision collision) {
        if (this.framesTillJump > 10) {
            this.isGrounded = true;
            this.inJump = false;
            this.framesTillJump = 0;
        }
    }


    private void AccelerateXZ() {

        if (!this.movementEnabled) {
            //return;
        }

        Vector3 resultingVector = Vector3.zero;

        if (Input.GetKey("w")) {
            Vector3 forward = GetMovementVectorInDirection(Vector3.forward);

            resultingVector += forward;
        }

        if (Input.GetKey("a")) {
            Vector3 left = GetMovementVectorInDirection(Vector3.left);

            resultingVector += left;
        }

        if (Input.GetKey("s")) {
            Vector3 back = GetMovementVectorInDirection(Vector3.back);

            resultingVector += back;
        }

        if (Input.GetKey("d")) {
            Vector3 right = GetMovementVectorInDirection(Vector3.right);

            resultingVector += right;
        }

        this.ManageSprinting();


        Vector3 normalizedSum = resultingVector.normalized;

        Vector3 scaledNormalizedResult = normalizedSum * this.movementAcceleration;

        this.rb.AddForce(scaledNormalizedResult, ForceMode.Acceleration);
    }


    private void ManageSprinting() {
        if (Input.GetKey("left shift")) {
            this.sprinting = true;
            this.fpsCamera.fieldOfView = Mathf.Lerp(this.standardFOV, this.sprintFOV, this.timeSinceSprintStarted);

            this.timeSinceSprintStarted += this.SprintFOVLerpFactor;
        }
        else {
            this.sprinting = false;
            this.fpsCamera.fieldOfView = Mathf.Lerp(this.sprintFOV, this.standardFOV, this.timeSinceSprintStarted);

            this.timeSinceSprintStarted -= this.SprintFOVLerpFactor;
        }

        this.timeSinceSprintStarted = Mathf.Clamp(this.timeSinceSprintStarted, 0, 1);
    }


    private Vector3 GetMovementVectorInDirection(Vector3 direction) {
        Vector3 rotated = this.transform.rotation * direction;

        switch (this.keyboardControlMode) {
            case MAKeyboardControlMode.CameraGoalAligned:
                rotated = this.currentCameraGoal.transform.rotation * direction;
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


    private void LimitSpeed() {
        //Limit the Player Speed because without it acceleration would result in infinite speed!
        Vector3 velocityXZ = Vector3.ProjectOnPlane(this.rb.velocity, Vector3.up);

        bool isToFast = false;
        float maxSpeedForCompare;

        float maxMovementSprintSpeed = this.maxMovementSprintSpeedFactor * this.maxMovementSpeed;


        switch (this.sprinting) {
            case true:
                isToFast = velocityXZ.magnitude > maxMovementSprintSpeed;
                maxSpeedForCompare = this.maxMovementSprintSpeedFactor;
                break;
            case false:
                isToFast = velocityXZ.magnitude > this.maxMovementSpeed;
                maxSpeedForCompare = this.maxMovementSpeed;

                break;
        }

        if (isToFast) {

            Vector3 newVelocityXZ = velocityXZ.normalized * maxSpeedForCompare;

            this.rb.velocity = new Vector3(newVelocityXZ.x, this.rb.velocity.y, newVelocityXZ.z);
        }
    }


    private void SlowDown() {

        if (!(Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")) && this.isGrounded) {
            Vector3 velocity = this.rb.velocity;

            velocity.x *= (1 - this.slowDownFactor);
            velocity.z *= (1 - this.slowDownFactor);

            this.rb.velocity = velocity;
        }
    }


    private void ManageJump() {

        if (Input.GetKeyDown("space") && this.isGrounded) {

            Vector3 force = Vector3.up * this.jumpForce;

            rb.AddForce(force, ForceMode.Impulse);

            this.transform.Translate(Vector3.up * 0.01f);
            this.isGrounded = false;
            this.inJump = true;
        }
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
        this.yRotator.transform.rotation = Quaternion.identity;
        this.yRotator.transform.rotation = Quaternion.Euler(0, 0, 4);
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

    
}
