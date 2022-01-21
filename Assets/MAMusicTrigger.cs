using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class MAMusicTrigger : MonoBehaviour {

    public AudioSource musicAudioSource;
    public UnityEvent musicOvertriggeredEvent;

    public bool playsMultipleTimes = false;
    public int playCounter;

    private bool isAlreadyPlayed = false;


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