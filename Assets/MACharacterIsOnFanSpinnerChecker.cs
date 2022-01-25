using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MACharacterIsOnFanSpinnerChecker : MonoBehaviour {
    private MAFanSpinner fanSpinner;


    void Start() {
        this.fanSpinner = this.transform.GetComponentInParent<MAFanSpinner>();
    }


    void OnCollisionStay(Collision characterCollider) {

        Debug.Log("character collides");
        MACharacterController characterController = characterCollider.transform.GetComponentInParent<MACharacterController>();
        this.fanSpinner.TransportCharacter(characterController);
    }

    void OnCollisionExit(Collision characterCollider) {

        this.fanSpinner.ReleaseCharacter();

    }
}
