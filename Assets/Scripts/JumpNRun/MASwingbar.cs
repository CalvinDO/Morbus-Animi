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

        this.currentRotator = new GameObject();
        this.currentRotator.name = "CurrentRotator";

        this.currentRotator.transform.SetPositionAndRotation(characterController.swingGrabPosition.position, characterController.swingGrabPosition.rotation);


        characterController.transform.SetParent(this.currentRotator.transform);
        this.currentRotator.transform.LookAt(this.transform);

        this.transform.LookAt(characterController.transform);
        this.currentRotator.transform.SetParent(this.transform);

        Vector3 handToSwingbar = this.transform.position - this.currentRotator.transform.position;
        this.currentRotator.transform.Translate(handToSwingbar);


        this.rb.centerOfMass = characterController.transform.position;
    }

    public void ReleaseCharacter(MACharacterController characterController) {

        characterController.transform.SetParent(null);
        characterController.rb.isKinematic = false;

        Vector3 tangentialVelocity = (this.last2CharacterFeetPositions[0] - this.last2CharacterFeetPositions[1]) * (1 / Time.deltaTime);
        characterController.rb.velocity = tangentialVelocity;

        GameObject.Destroy(this.currentRotator);
        this.currentRotator = null;
        this.attachedCharacter = null;
    }
}
