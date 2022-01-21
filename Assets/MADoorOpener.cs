using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MADoorOpener : MonoBehaviour {
    public MADoor door;

    [HideInInspector]
    public bool characterInReach = false;

    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (!this.characterInReach) {
            return;
        }

        if (Input.GetKeyUp(KeyCode.E)) {
            this.door.Open();
        }
    }

    void OnTriggerStay(Collider characterCollider) {
        this.characterInReach = true;
    }


    void OnTriggerExit(Collider characterCollider) {
        this.characterInReach = false;
    }
}
