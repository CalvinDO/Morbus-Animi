using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAStepUpper : MonoBehaviour {
    public Transform stepRayUpper;
    public Transform stepRayLower;

    [Range(0, 1)]
    public float stepHeight;
    [Range(0, .1f)]
    public float stepSpeed;

    [Range(0, .1f)]
    public float stepForwardSpeed;

    [Range(0, 1)]
    public float lowerRayDistance;
    [Range(0, 1)]
    public float upperRayDistance;

    [Range(0, .5f)]
    public float afterStepAnimationsDelay;

    private float remainingTimeAfterStepAnimationsDelay;

    private MACharacterController characterController;

    void Start() {
        this.characterController = this.transform.GetComponentInParent<MACharacterController>();

        this.remainingTimeAfterStepAnimationsDelay = this.afterStepAnimationsDelay;
    }

    private void Awake() {
        this.stepRayUpper.localPosition = new Vector3(this.stepRayUpper.localPosition.x, this.stepHeight, this.stepRayUpper.localPosition.z);
    }

    // Update is called once per frame
    void FixedUpdate() {
        this.StepClimb();
    }

    void Update() {
        if (!this.characterController.isSteppingUp) {
            return;
        }

        this.remainingTimeAfterStepAnimationsDelay -= Time.deltaTime;
    }

    private void StepClimb() {

        LayerMask mask = LayerMask.GetMask("Default",  "MA_NavMesh", "LayerMask", "SeeThrough");

        Debug.DrawRay(this.stepRayLower.position, this.transform.forward * this.lowerRayDistance, Color.red);
        Debug.DrawRay(this.stepRayUpper.position, this.transform.forward * this.upperRayDistance, Color.blue);



        if (Physics.Raycast(this.stepRayLower.position, this.transform.forward, out _, this.lowerRayDistance, mask)) {


            if (!Physics.Raycast(this.stepRayUpper.position, this.transform.forward, out _, this.upperRayDistance, mask)) {

                float currentStepSpeed = this.stepSpeed * (this.characterController.isSprinting ? 2 : 1);

                this.characterController.transform.position += new Vector3(0, currentStepSpeed) + this.stepForwardSpeed * this.transform.forward;
                this.characterController.isGrounded = true;

                this.characterController.isSteppingUp = true;
                this.remainingTimeAfterStepAnimationsDelay = this.afterStepAnimationsDelay;

                return;
            }
        }

        if (Physics.Raycast(this.stepRayLower.position, this.transform.TransformDirection(1.5f, 0, 1f), out _, this.lowerRayDistance, mask)) {


            if (!Physics.Raycast(this.stepRayUpper.position, this.transform.forward, out _, this.upperRayDistance, mask)) {

                float currentStepSpeed = this.stepSpeed * (this.characterController.isSprinting ? 2 : 1);

                this.characterController.transform.position += new Vector3(0, currentStepSpeed) + this.stepForwardSpeed * this.transform.forward;
                this.characterController.isGrounded = true;

                this.characterController.isSteppingUp = true;
                this.remainingTimeAfterStepAnimationsDelay = this.afterStepAnimationsDelay;

                return;
            }
        }



        if (Physics.Raycast(this.stepRayLower.position, this.transform.TransformDirection(-1.5f, 0, 1f), out _, this.lowerRayDistance, mask)) {


            if (!Physics.Raycast(this.stepRayUpper.position, this.transform.forward, out _, this.upperRayDistance, mask)) {

                float currentStepSpeed = this.stepSpeed * (this.characterController.isSprinting ? 2 : 1);

                this.characterController.transform.position += new Vector3(0, currentStepSpeed) + this.stepForwardSpeed * this.transform.forward;
                this.characterController.isGrounded = true;

                this.characterController.isSteppingUp = true;
                this.remainingTimeAfterStepAnimationsDelay = this.afterStepAnimationsDelay;

                return;
            }
        }




        if (this.remainingTimeAfterStepAnimationsDelay <= 0) {
            this.characterController.isSteppingUp = false;
            this.remainingTimeAfterStepAnimationsDelay = this.afterStepAnimationsDelay;
        }
    }
}
