using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAGroundCheck : MonoBehaviour
{
    private MACharacterController characterController;

    void Start()
    {
        this.characterController = this.transform.root.GetComponent<MACharacterController>();
    }

    private void OnTriggerStay(Collider other) {
        this.characterController.Ground();
    }

    private void OnTriggerExit(Collider other) {
        this.characterController.DeGround();
    }
}
