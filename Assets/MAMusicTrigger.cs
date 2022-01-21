using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class MAMusicTrigger : MonoBehaviour {

    public AudioSource musicAudioSource;

    public bool playsMultipleTimes = false;
    public int playCounter;

    private bool isAlreadyPlayed = false;


    void Update() {

        if (this.playsMultipleTimes) {
            return;
        }

        if (!this.musicAudioSource.isPlaying) {

        }
    }

    void OnTriggerEnter(Collider other) {

        if (this.playsMultipleTimes) {

            this.playCounter--;

            if (this.playCounter >= 0) {
                this.musicAudioSource.Play();
            }
        }
        else {
            if (!this.isAlreadyPlayed) {
                this.musicAudioSource.Play();
                this.isAlreadyPlayed = true;

            }
        }
    }
}