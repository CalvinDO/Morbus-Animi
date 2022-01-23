using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAStepUpper : MonoBehaviour {
    public Transform stepRayUpper;
    public Transform stepRayLower;

    [Range(0, 1)]
    public float stepHeight;
    [Range(0, 1)]
    public float stepSpeed;

    private MACharacterController characterController;

    void Start() {
        this.characterController = this.transform.root.GetComponent<MACharacterController>();
    }

    private void Awake() {
        this.stepRayUpper.position = new Vector3(this.stepRayUpper.position.x, this.stepHeight, this.stepRayUpper.position.z);
    }

    // Update is called once per frame
    void FixedUpdate() {
        this.StepClimb();
    }

    private void StepClimb() {
        RaycastHit hitLower;

        if (Physics.Raycast(this.stepRayLower.position, this.transform.forward, out hitLower, 0.1f)) {
            RaycastHit hitUpper;

            Debug.Log("collider lower");
            if (!Physics.Raycast(this.stepRayUpper.position, this.transform.forward, out hitUpper, 0.2f)) {
                this.characterController.transform.position += new Vector3(0, this.stepSpeed);
                Debug.Log("not collider upper");

            }
        }
    }
}
