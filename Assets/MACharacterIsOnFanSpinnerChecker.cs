using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MACharacterIsOnFanSpinnerChecker : MonoBehaviour {
    private MAFanSpinner fanSpinner;


    void Start() {
        this.fanSpinner = this.transform.GetComponentInParent<MAFanSpinner>();
    }

    void OnTriggerStay(Collider characterCollider) {

       MACharacterController characterController = characterCollider.transform.GetComponentInParent<MACharacterController>();
        this.fanSpinner.TransportCharacter(characterController);
      

     
    }

    void OnTriggerExit(Collider characterCollider) {

        this.fanSpinner.ReleaseCharacter();

    }
}
