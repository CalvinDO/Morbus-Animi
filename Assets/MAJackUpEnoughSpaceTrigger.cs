using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAJackUpEnoughSpaceTrigger : MonoBehaviour {
    public MAJackUpper jackUpper;


    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerStay(Collider other) {

        if (other.isTrigger) {
            return;
        }

        if (other.transform.root.GetComponent<MACharacterController>()) {
            return;
        }

        this.jackUpper.SetSpaceOccupied();
    }

    private void OnTriggerExit(Collider other) {
        Debug.Log(other.gameObject.name);

        if (other.isTrigger) {
            return;
        }

        if (other.transform.root.GetComponent<MACharacterController>()) {
            return;
        }

        this.jackUpper.SetSpaceFree();
    }
}
