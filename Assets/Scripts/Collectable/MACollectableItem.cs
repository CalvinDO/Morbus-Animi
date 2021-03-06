using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class MACollectableItem : MonoBehaviour {


    public MAItem item;

    private bool isCharacterInReach = false;

    private bool isAlreadyCollected = false;


    public AudioSource collectAudio;
    public AudioClip pickupSound;


    public Transform physicalItem;


    public Canvas indicator;

    public UnityEvent triggeredEvent;


    void Update() {

        if (!this.isCharacterInReach) {
            this.indicator.gameObject.SetActive(false);

            return;
        }

        this.indicator.gameObject.SetActive(true);

        if (Input.GetKeyUp(KeyCode.E)) {
            this.Collect();
        }

    }

    private void Collect() {

        MAInventory.instance.Add(item);

        this.physicalItem.gameObject.SetActive(false);
        this.isAlreadyCollected = true;

        this.collectAudio.PlayOneShot(this.pickupSound);

        GameObject.FindObjectOfType<MACharacterController>().GetComponent<MACharacterController>().animator.SetTrigger("interact");

        MAUICurrentObjectiveDisplay.instance.IncreaseObjectiveIndex();
        /*
        if (this.triggeredEvent == null) {
            return;
        }
        this.triggeredEvent.Invoke();
        */
    }

    void OnTriggerStay(Collider characterCollider) {
        this.isCharacterInReach = true;

    }

    void OnTriggerExit(Collider characterCollider) {
        this.isCharacterInReach = false;

    }
}
