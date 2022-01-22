using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MAUIAppear : MonoBehaviour {
    [SerializeField] private Image customImage;

    void OnTriggerEnter(Collider other) {

        customImage.enabled = true;

    }


    void OnTriggerExit(Collider other) {

        customImage.enabled = false;

    }

}
