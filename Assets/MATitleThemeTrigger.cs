using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MATitleThemeTrigger : MonoBehaviour {

    public AudioSource musicAudioSource;
    private bool isAlreadyPlayed = false;

    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider other) {

        if (!this.isAlreadyPlayed) {
            this.musicAudioSource.Play();
            this.isAlreadyPlayed = true;
        }
    }
}
