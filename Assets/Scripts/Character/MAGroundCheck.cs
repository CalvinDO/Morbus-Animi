using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAGroundCheck : MonoBehaviour {

    private MACharacterController characterController;

    public Transform[] raycastPositions;


    void Start() {
        this.characterController = this.transform.root.GetComponent<MACharacterController>();
    }

    private void Update() {

        foreach (Transform raycastTransform in this.raycastPositions) {

            Ray ray = new Ray(raycastTransform.position, Vector3.down);

            if (Physics.Raycast(ray, out _, 0.1f)) {

                this.characterController.Ground();
                return;
            }
        }

        this.characterController.DeGround();


    }
}
