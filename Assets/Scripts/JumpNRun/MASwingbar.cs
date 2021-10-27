using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MASwingbar : MonoBehaviour {
    public Rigidbody rb;
    private GameObject currentRotator;

    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetCharacterInitialPosition(MACharacterController characterController) {

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
        GameObject.Destroy(this.currentRotator);
        this.currentRotator = null;
    }
}
