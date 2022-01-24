using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MADeathZone : MonoBehaviour {

    void OnTriggerEnter(Collider characterCollider) {
        MACharacterController characterController = characterCollider.transform.GetComponentInParent<MACharacterController>();

        if (characterController.isDieing) {
            return;
        }

        characterController.Die();
    }

    void OnCollisionEnter(Collision characterCollision) {
        MACharacterController characterController = characterCollision.transform.GetComponentInParent<MACharacterController>();

        if (characterController.isDieing) {
            return;
        }

        characterController.Die();
    }
}
