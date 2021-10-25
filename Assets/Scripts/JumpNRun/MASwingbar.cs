using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MASwingbar : MonoBehaviour {
    public Rigidbody rb;


    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetCharacterInitialPosition(MACharacterController characterController) {

        characterController.rb.isKinematic = true;

        GameObject characterRotator = new GameObject();
        characterRotator.name = "CharacterRotator";

        characterRotator.transform.SetPositionAndRotation(characterController.swingGrabPosition.position, characterController.swingGrabPosition.rotation);

        characterRotator.transform.SetParent(this.transform);

        characterController.transform.SetParent(characterRotator.transform);
        characterRotator.transform.LookAt(this.transform);

        Vector3 handToSwingbar = this.transform.position - characterRotator.transform.position;
        characterRotator.transform.Translate(handToSwingbar);


        this.rb.centerOfMass = characterController.transform.position;
    }
}
