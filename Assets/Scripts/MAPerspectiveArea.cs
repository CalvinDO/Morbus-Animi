using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAPerspectiveArea : MonoBehaviour {
    public Collider trigger;
    public bool isFixed = false;
    public GameObject fixedCameraTarget;
    public MACameraDirection cameraDirection;
    public MASpaceType spaceType;

    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {

        if (!other.GetComponent<MACharacterController>()) {
            return;
        }

        MACharacterController characterController = other.GetComponent<MACharacterController>();

        //All Cam Areas!
        characterController.spaceType = this.spaceType;

        //Fixed Cam Area!
        if (this.isFixed) {
            characterController.cameraMode = MACameraMode.Fixed;
            characterController.fixedCameraGoal = this.fixedCameraTarget.transform;
            characterController.keyboardControlMode = MAKeyboardControlMode.CameraCurrentAligned;
            return;
        }

        //Folow Cam Area!
        characterController.cameraMode = MACameraMode.ThirdPerson;
        switch (this.cameraDirection) {
            case MACameraDirection.Back:
                // characterController.goalRotator.rotation = Quaternion.Euler(new Vector3(0, 0));
                this.RotateGoalRotator(characterController.goalRotator, 0);
                break;
            case MACameraDirection.Left:
                //characterController.goalRotator.rotation = Quaternion.Euler(new Vector3(0, 90));
                this.RotateGoalRotator(characterController.goalRotator, 90);
                break;
            case MACameraDirection.Front:
                // characterController.goalRotator.rotation = Quaternion.Euler(new Vector3(0, 180));
                this.RotateGoalRotator(characterController.goalRotator, 180);
                break;
            case MACameraDirection.Right:
                // characterController.goalRotator.rotation = Quaternion.Euler(new Vector3(0, 270));
                this.RotateGoalRotator(characterController.goalRotator, 270);
                break;
            default:
                characterController.goalRotator.rotation = Quaternion.identity;
                break;
        }

    }

    private void RotateGoalRotator(Transform goalRotator, float angle) {
        goalRotator.rotation = Quaternion.identity;
        goalRotator.Rotate(new Vector3(0, angle));
    }



    private void OnTriggerExit(Collider other) {
        if (!other.GetComponent<MACharacterController>()) {
            return;
        }

        MACharacterController characterController = other.GetComponent<MACharacterController>();

        characterController.SetDefaultModeAndSpaceType();
    }
}
