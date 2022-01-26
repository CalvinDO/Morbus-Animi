using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MAUIPicturePrefab : MonoBehaviour {
    public Canvas canvas;
    public Image panel;
    public Text text;

    public MAMainMenu mainMenu;

    public bool isShowingPicture = false;



    public AudioSource audioSource;
    public AudioClip openSound;


    public void ShowPicture(MAPicture picture) {

        this.isShowingPicture = true;

        this.panel.GetComponent<Image>().sprite = picture.icon;
        this.text.text = picture.description;


        this.canvas.gameObject.SetActive(true);

        this.mainMenu.PauseGame();



        return;
    }

    public void Close() {

        if (!this.isShowingPicture) {
            return;
        }

        this.isShowingPicture = false;

        this.canvas.gameObject.SetActive(false);
        this.transform.root.GetComponentInChildren<MAMainMenu>().ResumeGame();

        this.audioSource.PlayOneShot(this.openSound);


        MAHintPage.isCurrentlyOpened = false;

        // Cursor.lockState = CursorLockMode.Locked;

    }
}
