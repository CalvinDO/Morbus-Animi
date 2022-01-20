using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class MAMusicTrigger : MonoBehaviour {

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