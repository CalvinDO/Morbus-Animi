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


    public virtual void Update() {
        if (Input.GetKeyUp(KeyCode.E) && this.isCharacterInTrigger) {
            this.uiPicturePrefab = GameObject.Find("PictureUIPrefab").GetComponent<MAUIPicturePrefab>();
            this.uiPicturePrefab.ShowPicture(this.picture);

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

    public override void MAInteract() {

        this.clearText();
        base.MAInteract();

    }
}
