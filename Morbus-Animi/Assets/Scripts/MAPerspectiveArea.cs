using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAPerspectiveArea : MonoBehaviour {
    public Collider trigger;
    public bool isFixed = false;
    public GameObject fixedCameraTarget;
    public MACameraDirection cameraDirection;


    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {

        if (other.GetComponent<MACharacterController>()) {

            MACharacterController characterController = other.GetComponent<MACharacterController>();

            if (this.isFixed) {
                characterController.cameraMode = MACameraMode.Fixed;
                characterController.fixedCameraGoal = this.fixedCameraTarget.transform;
            }
            else {
                characterController.cameraMode = MACameraMode.ThirdPerson;
                switch (this.cameraDirection) {
                    case MACameraDirection.Back:
                       // characterController.goalRotator.rotation = Quaternion.Euler(new Vector3(0, 0));
                        this.SetYRotation(characterController.goalRotator, 0);
                        break;
                    case MACameraDirection.Left:
                        //characterController.goalRotator.rotation = Quaternion.Euler(new Vector3(0, 90));
                        this.SetYRotation(characterController.goalRotator, 90);
                        break;
                    case MACameraDirection.Front:
                       // characterController.goalRotator.rotation = Quaternion.Euler(new Vector3(0, 180));
                        this.SetYRotation(characterController.goalRotator,  180);
                        break;
                    case MACameraDirection.Right:
                       // characterController.goalRotator.rotation = Quaternion.Euler(new Vector3(0, 270));
                        this.SetYRotation(characterController.goalRotator,  270);
                        break;
                    default:
                        characterController.goalRotator.rotation = Quaternion.Euler(new Vector3(0, 0));
                        break;
                }
            }
        }
    }

    private void SetYRotation(Transform transform, float angle) {
        transform.rotation = Quaternion.identity;
        transform.Rotate(new Vector3(0, angle));
    }


    /*
    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<MACharacterController>()) {
            MACharacterController characterController = other.GetComponent<MACharacterController>();

            characterController.cameraMode = MACameraMode.ThirdPerson;

        }
    }
    */
}
