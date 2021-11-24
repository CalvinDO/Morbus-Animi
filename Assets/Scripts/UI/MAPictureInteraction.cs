using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MAPictureInteraction : MAInteractable {


    public MAPicture picture;

    private MAUIPicturePrefab uiPicturePrefab;

    private bool isCharacterInTrigger = false;


    private void Start() {

       

    }

    private void Update() {
        if (Input.GetKey(KeyCode.E) && this.isCharacterInTrigger) {
            this.uiPicturePrefab = GameObject.Find("PictureUIPrefab").GetComponent<MAUIPicturePrefab>();
            this.uiPicturePrefab.showPicture(this.picture);
            MAInventory.instance.AddPicture(this.picture);

            GameObject.Destroy(this.gameObject);
        }
    }
    public void showNorah() {
        //this.darkNorah.gameObject.SetActive(true);

        // this.darkNorah.StartMoving();
    }

    private void OnTriggerStay(Collider other) {

        if (!other.CompareTag("MainCollider")) {
            return;
        }

        this.isCharacterInTrigger = true;

        this.MAInteract();
    }
    public override void MAInteract() {
        this.clearText();
        base.MAInteract();


        /*
        if (this.darkNorah != null) {
            canvas.GetComponentInChildren<Button>().onClick.AddListener(this.showNorah);
        }
        */
    }
}
