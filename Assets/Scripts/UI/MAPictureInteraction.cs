using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MAPictureInteraction : MAInteractable {


    public MAPicture picture;

    [HideInInspector]
    public MAUIPicturePrefab uiPicturePrefab;

    [HideInInspector]
    public bool isCharacterInTrigger = false;

    public AudioSource audioSource;
    public AudioClip openSound;


    public virtual void Update() {
        if (Input.GetKeyUp(KeyCode.E) && this.isCharacterInTrigger) {
            this.uiPicturePrefab = GameObject.Find("PictureUIPrefab").GetComponent<MAUIPicturePrefab>();
            this.uiPicturePrefab.ShowPicture(this.picture);

            if (this.audioSource != null) {
                if (this.openSound != null) {
                    this.audioSource.PlayOneShot(this.openSound);
                }
            }

            this.DoCollectActions();

        }
    }

    public virtual void DoCollectActions() {
        MAInventory.instance.AddPicture(this.picture);

        GameObject.Destroy(this.gameObject);
    }

    void OnDrawGizmosSelected() {
    }

    private void OnTriggerStay(Collider other) {

        this.isCharacterInTrigger = true;

        this.MAInteract();
    }

    private void OnTriggerExit(Collider other) {

        this.isCharacterInTrigger = false;
    }

    public override void MAInteract() {

        this.clearText();
        base.MAInteract();

    }
}
