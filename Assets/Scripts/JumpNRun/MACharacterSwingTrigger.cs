using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MACharacterSwingTrigger : MonoBehaviour {

    public bool isSwingbarReachable = false;
    public MASwingbar reachableSwingbar;
    private MACharacterController characterController;

    void Start() {
        this.characterController = this.transform.root.GetComponent<MACharacterController>();
    }

    private void OnTriggerStay(Collider other) {

        if (this.characterController.IsPerformingSoloJumpNRunMove) {
            return;
        }

        if (other.gameObject.CompareTag("SwingbarTrigger")) {
            this.isSwingbarReachable = true;
            this.reachableSwingbar = other.transform.root.GetComponentInChildren<MASwingbar>();
            Time.timeScale = 0.33f;
        }
    }


    private void OnTriggerExit(Collider other) {

        if (this.characterController.IsPerformingSoloJumpNRunMove) {
            return;
        }

        if (other.gameObject.CompareTag("SwingbarTrigger")) {
            this.isSwingbarReachable = false;
            this.reachableSwingbar = null;
            Time.timeScale = 1;
        }
    }
}
