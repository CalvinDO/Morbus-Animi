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

    private void Update() {

        Ray ray = new Ray(this.transform.position, Vector3.down);

        if (Physics.Raycast(ray, out _, 0.1f)) {
            this.characterController.Ground();
        }
        else {
            this.characterController.DeGround();
        }
    }
}
