using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MASwingbar : MonoBehaviour {
    public Rigidbody rb;
    private GameObject currentRotator;



    void Start() {
        // this.InitializeCharacterFeetPositions();
    }


    void Update() {

    }

    private void InitializeCharacterFeetPositions() {
        //this.last2CharacterFeetPositions = new Vector3[2];
        //this.last2CharacterFeetPositions[0] = Vector3.zero;
        //this.last2CharacterFeetPositions[1] = this.last2CharacterFeetPositions[0];
    }


    public void StartSwingAndSetCharacterInitialPosition(MACharacterController characterController) {

        this.rb.isKinematic = false;

        characterController.SetPerformingSoloJumpNRunMove(true);

        //prepare Character values
        characterController.rb.isKinematic = true;
        characterController.movementEnabled = false;

        //set character to y z plane of swingbar for centering

        characterController.transform.position -= this.transform.right * this.transform.InverseTransformPoint(characterController.transform.position).x;


        characterController.physicalBody.transform.rotation = Quaternion.identity;

        Vector3 pointToLookAt = new Vector3(this.transform.position.x, characterController.transform.position.y, this.transform.position.z);


        characterController.transform.LookAt(pointToLookAt);



        this.currentRotator = new GameObject();
        this.currentRotator.name = "CurrentRotator";


        //Rotate Character so he points at the bar

        this.currentRotator.transform.SetPositionAndRotation(characterController.swingCOM.position, characterController.swingCOM.rotation);
        characterController.transform.SetParent(this.currentRotator.transform);
        this.currentRotator.transform.LookAt(this.transform);


        //move Character so arms grab the bar

        characterController.transform.SetParent(null);
        this.currentRotator.transform.SetPositionAndRotation(characterController.swingGrabPosition.position, characterController.swingGrabPosition.rotation);
        characterController.transform.SetParent(this.currentRotator.transform);
        this.currentRotator.transform.position = this.transform.position;


        this.RotateSwingTowardsCharacter(characterController);


        this.currentRotator.transform.SetParent(this.transform);


        //this.rb.centerOfMass = characterController.transform.position;
    }


    public void RotateSwingTowardsCharacter(MACharacterController characterController) {

        if (Vector3.Dot(this.transform.forward, this.currentRotator.transform.forward) > 0) {
            this.transform.Rotate(new Vector3(this.currentRotator.transform.eulerAngles.x + 90, 0));
            return;
        }

        this.transform.Rotate(new Vector3(-(this.currentRotator.transform.eulerAngles.x + 90), 0));
    }

    public void ReleaseCharacter(MACharacterController characterController) {

        characterController.SetPerformingSoloJumpNRunMove(false);

        characterController.transform.SetParent(null);
        characterController.transform.rotation = Quaternion.identity;
        characterController.rb.isKinematic = false;
        characterController.movementEnabled = true;


        Vector3 r = characterController.swingCOM.position - this.transform.position;
        Vector3 angularVelocity = this.rb.angularVelocity;
        Vector3 tangentialVelocity = Vector3.Cross(angularVelocity, r) * characterController.swingReleaseVelocityFactor;


        characterController.rb.velocity = tangentialVelocity;


        GameObject.Destroy(this.currentRotator);
        this.currentRotator = null;

        this.rb.angularVelocity = Vector3.zero;
        this.rb.velocity = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;

        // this.InitializeCharacterFeetPositions();

        this.rb.isKinematic = true;

    }
}
