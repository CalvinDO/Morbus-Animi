using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAClimb : MonoBehaviour
{
    public bool characterInTrigger = false;

    public MACharacterController controller;

    void Update()
    {
        if (this.characterInTrigger) {
            this.MoveUp();
        }
    }

    void OnTriggerStay(Collider other) {


        this.characterInTrigger = true;
        this.controller = other.transform.GetComponentInParent<MACharacterController>();

    }


    void MoveUp() {
        if (Input.GetKey(KeyCode.E)) {
            this.controller.transform.Translate(Vector3.up * Time.deltaTime * this.controller.climbSpeed);
        }
    }


    void OnTriggerExit(Collider other) {
        this.characterInTrigger = false;
    }
}
