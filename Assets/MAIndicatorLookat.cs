using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAIndicatorLookat : MonoBehaviour {

    MACharacterController characterController;

    void Start() {
        this.characterController = GameObject.FindObjectOfType<MACharacterController>();
    }

    void Awake() {
        this.characterController = GameObject.FindObjectOfType<MACharacterController>();
    }

    // Update is called once per frame
    void Update() {
        if (this.characterController != null) {
            this.transform.LookAt(this.characterController.mainCamera.transform);
            this.transform.Rotate(Vector3.up * 180);
        }
    }
}
