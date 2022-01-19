using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MADeathZone : MonoBehaviour {

    void OnTriggerEnter(Collider characterCollider) {
        characterCollider.transform.root.GetComponent<MACharacterController>().Die();
    }
}
