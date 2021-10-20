using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MACharacterSwingTrigger : MonoBehaviour {

    public bool isSwingbarReachable = false;
    public MASwingbar reachableSwingbar;


    private void OnTriggerStay(Collider other) {

        if (other.gameObject.CompareTag("SwingbarTrigger")) {
            this.isSwingbarReachable = true;
            this.reachableSwingbar = other.transform.root.GetComponentInChildren<MASwingbar>();
        }
    }


    private void OnTriggerExit(Collider other) {

        if (other.gameObject.CompareTag("SwingbarTrigger")) {
            this.isSwingbarReachable = false;
            this.reachableSwingbar = null;
        }
    }
}
