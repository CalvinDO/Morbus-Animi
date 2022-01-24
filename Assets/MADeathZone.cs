using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MADeathZone : MonoBehaviour {

    void OnTriggerEnter(Collider characterCollider) {
        MACharacterController characterController = characterCollider.transform.root.GetComponent<MACharacterController>();

        if (characterController.isDieing) {
            return;
        }

        characterController.Die();
    }

    void OnCollisionEnter(Collision characterCollision) {
        MACharacterController characterController = characterCollision.transform.root.GetComponent<MACharacterController>();

        if (characterController.isDieing) {
            return;
        }

        characterController.Die();
    }
}
