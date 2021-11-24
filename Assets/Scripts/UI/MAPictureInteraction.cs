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

        Debug.Log("interact!");

        this.MAInteract();
    }
    public override void MAInteract() {
        Debug.Log("interact!");

        this.clearText();
        base.MAInteract();

        this.uiPicturePrefab.panel.GetComponent<Image>().sprite = picture.icon;
        this.uiPicturePrefab.text.text = picture.description;

        /*
        if (this.darkNorah != null) {
            canvas.GetComponentInChildren<Button>().onClick.AddListener(this.showNorah);
        }
        */

        this.uiPicturePrefab.canvas.gameObject.SetActive(!this.uiPicturePrefab.canvas.gameObject.activeSelf);
        if (this.uiPicturePrefab.canvas.gameObject.activeSelf) {
            Time.timeScale = 0;
        }
        else {
            Time.timeScale = 1;
        }
    }
}
