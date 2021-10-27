using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MASwingbar : MonoBehaviour {
    public Rigidbody rb;
    private GameObject currentRotator;
    private MACharacterController attachedCharacter;

    private Vector3[] last2CharacterFeetPositions;

    void Start() {
        //initialize empty
        this.last2CharacterFeetPositions = new Vector3[2];
        this.last2CharacterFeetPositions[0] = Vector3.zero;
        this.last2CharacterFeetPositions[1] = this.last2CharacterFeetPositions[0];

    }


    void Update() {

        if (this.attachedCharacter == null) {
            return;
        }

        this.last2CharacterFeetPositions[1] = this.last2CharacterFeetPositions[0];
        this.last2CharacterFeetPositions[0] = this.attachedCharacter.transform.position;
    }

    public void SetCharacterInitialPosition(MACharacterController characterController) {

        this.attachedCharacter = characterController;

        characterController.rb.isKinematic = true;
        Vector3 oldPos = characterController.transform.position;
        characterController.transform.position += Vector3.right * (this.transform.position.x - characterController.transform.position.x);
        characterController.transform.rotation = Quaternion.identity;



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


        this.RotateSwingTowardsCharacter(characterController);


        this.currentRotator.transform.SetParent(this.transform);


        //this.rb.centerOfMass = characterController.transform.position;
    }

    public void RotateSwingTowardsCharacter(MACharacterController characterController) {

        this.transform.localRotation = Quaternion.identity;
        this.rb.angularVelocity = Vector3.zero;

        this.transform.Rotate(new Vector3(this.currentRotator.transform.eulerAngles.x + 90, 0));
    }

    public void ReleaseCharacter(MACharacterController characterController) {

        characterController.transform.SetParent(null);
        characterController.transform.rotation = Quaternion.identity;
        characterController.rb.isKinematic = false;

      //  this.transform.LookAt(Vector3.down);

        Vector3 tangentialVelocity = (this.last2CharacterFeetPositions[0] - this.last2CharacterFeetPositions[1]) * (1 / Time.deltaTime);
        characterController.rb.velocity = tangentialVelocity;

        GameObject.Destroy(this.currentRotator);
        this.currentRotator = null;
        this.attachedCharacter = null;
    }
}
