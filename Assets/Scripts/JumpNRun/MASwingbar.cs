using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MASwingbar : MonoBehaviour {
    public Rigidbody rb;
    private GameObject currentRotator;
    private MACharacterController attachedCharacter;


    void Start() {
        this.InitializeCharacterFeetPositions();
    }


    void Update() {

    }

    private void InitializeCharacterFeetPositions() {
        //this.last2CharacterFeetPositions = new Vector3[2];
        //this.last2CharacterFeetPositions[0] = Vector3.zero;
        //this.last2CharacterFeetPositions[1] = this.last2CharacterFeetPositions[0];
    }


    public void SetCharacterInitialPosition(MACharacterController characterController) {

        this.rb.isKinematic = false;

        this.attachedCharacter = characterController;

        //prepare Character values
        characterController.rb.isKinematic = true;
        Vector3 oldPos = characterController.transform.position;
        characterController.transform.position += Vector3.right * (this.transform.position.x - characterController.transform.position.x);


        float dot = Vector3.Dot(characterController.physicalBody.transform.forward, this.transform.up);

        characterController.transform.rotation = Quaternion.identity;

        if (dot < 0) {
            characterController.physicalBody.transform.Rotate(Vector3.up * 180);
        }


        this.currentRotator = new GameObject();
        this.currentRotator.name = "CurrentRotator";


        //Rotate Character so he points at the bar

        this.currentRotator.transform.SetPositionAndRotation(characterController.swingFootUpPosition.position, characterController.swingFootUpPosition.rotation);
        characterController.transform.SetParent(this.currentRotator.transform);
        this.currentRotator.transform.LookAt(this.transform);


        //move Character so arms grab the bar

        characterController.transform.SetParent(null);
        this.currentRotator.transform.SetPositionAndRotation(characterController.swingGrabPosition.position, characterController.swingGrabPosition.rotation);
        characterController.transform.SetParent(this.currentRotator.transform);
        this.currentRotator.transform.position = this.transform.position;
        characterController.movementEnabled = false;


        this.RotateSwingTowardsCharacter(characterController);


        this.currentRotator.transform.SetParent(this.transform);


        //this.rb.centerOfMass = characterController.transform.position;
    }

    public void RotateSwingTowardsCharacter(MACharacterController characterController) {

   
      
        if (this.currentRotator.transform.eulerAngles.y != 0) {
            this.transform.Rotate(new Vector3(-(this.currentRotator.transform.eulerAngles.x + 90), 0));
            return;
        }

        this.transform.Rotate(new Vector3(this.currentRotator.transform.eulerAngles.x + 90, 0));
    }

    public void ReleaseCharacter(MACharacterController characterController) {

        characterController.transform.SetParent(null);
        characterController.transform.rotation = Quaternion.identity;
        characterController.rb.isKinematic = false;
        characterController.movementEnabled = true;


        Vector3 r = characterController.swingFootUpPosition.position - this.transform.position;
        Vector3 angularVelocity = this.rb.angularVelocity;
        Vector3 tangentialVelocity = Vector3.Cross(angularVelocity, r) * characterController.swingReleaseVelocityFactor;

        characterController.rb.velocity = tangentialVelocity;


        GameObject.Destroy(this.currentRotator);
        this.currentRotator = null;
        this.attachedCharacter = null;

        this.rb.angularVelocity = Vector3.zero;
        this.rb.velocity = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;

        this.InitializeCharacterFeetPositions();

        this.rb.isKinematic = true;

    }
}
