using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class MAWindZone : MonoBehaviour {

    [Range(0, 500)]
    public float pushForce;

    public bool isRunnning = true;

    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerStay(Collider other) {

        if (!this.isRunnning) {
            return;
        }

        MACharacterController characterController = other.transform.root.GetComponent<MACharacterController>();


        if (characterController == null) {
            return;
        }

        characterController.rb.AddForce(this.transform.forward * this.pushForce, ForceMode.Force);
    }
}
