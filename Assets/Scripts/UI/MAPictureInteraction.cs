using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MAPictureInteraction : MAInteractable {


    public MAPicture picture;

    private MAUIPicturePrefab uiPicturePrefab;

    private void Start() {

        this.uiPicturePrefab = GameObject.Find("PicturePrefab").GetComponent<MAUIPicturePrefab>();

    }

    public void showNorah() {
        //this.darkNorah.gameObject.SetActive(true);

        // this.darkNorah.StartMoving();
    }

    private void OnTriggerEnter(Collider other) {

        Debug.Log(other.gameObject.name);

        if (!other.CompareTag("MainCollider")) {
            return;
        }


        this.MAInteract();
    }
    public override void MAInteract() {
        this.clearText();
        base.MAInteract();


        this.uiPicturePrefab.showPicture(this.picture);

        /*
        if (this.darkNorah != null) {
            canvas.GetComponentInChildren<Button>().onClick.AddListener(this.showNorah);
        }
        */

        MAInventory.instance.Add(this.picture);

        GameObject.Destroy(this.gameObject);
    }
}
