using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class MACollectableItem : MonoBehaviour
{


    public MAItem item;

    private bool isCharacterInReach = false;

    private bool isAlreadyCollected = false;


    public AudioSource collectAudio;
    public AudioClip keyPickupSound;

    // Update is called once per frame
    void Update()
    {
        if (!this.isCharacterInReach) {
            return;
        }

        if (Input.GetKeyUp(KeyCode.E)) {
            MAInventory.instance.Add(item);

            this.gameObject.SetActive(false);
            this.isAlreadyCollected = true;

            this.collectAudio.PlayOneShot(this.keyPickupSound);
        }


    }


    void OnTriggerStay(Collider characterCollider) {
        this.isCharacterInReach = true;

    }

    void OnTriggerExit(Collider characterCollider) {
        this.isCharacterInReach = false;

    }
}
