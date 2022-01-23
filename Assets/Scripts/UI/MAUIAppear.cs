using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MAUIAppear : MonoBehaviour {
    [SerializeField] private Image customImage;


    public bool isCharacterInReach = false;


    void OnTriggerStay(Collider other) {

        this.isCharacterInReach = true;
    }


    void OnTriggerExit(Collider other) {

        this.isCharacterInReach = false;

    }

    void Update() {

        if (!this.gameObject.activeSelf) {
            return;
        }
        if (this.isCharacterInReach) {

            this.customImage.enabled = true;
        }
        else {
            this.customImage.enabled = false;
        }
    }

}
