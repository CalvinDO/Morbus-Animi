using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class MADoorOpener : MonoBehaviour {

    public MADoor door;

    [HideInInspector]
    public bool isCharacterInReach = false;

    public Canvas indicator;

    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (!this.isCharacterInReach) {
            this.indicator.gameObject.SetActive(false);

            return;
        }

        this.indicator.gameObject.SetActive(true);

        if (Input.GetKeyUp(KeyCode.E)) {
            this.door.Open();
        }
    }

    void OnTriggerStay(Collider characterCollider) {
        this.isCharacterInReach = true;
    }


    void OnTriggerExit(Collider characterCollider) {
        this.isCharacterInReach = false;
    }
}
