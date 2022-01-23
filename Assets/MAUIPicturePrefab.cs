using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MAUIPicturePrefab : MonoBehaviour {
    public Canvas canvas;
    public Image panel;
    public Text text;

    public MAMainMenu mainMenu;

    public void ShowPicture(MAPicture picture) {
        this.panel.GetComponent<Image>().sprite = picture.icon;
        this.text.text = picture.description;


        this.canvas.gameObject.SetActive(true);

        this.mainMenu.PauseGame();

        return;
    }

    public void Close() {
        this.canvas.gameObject.SetActive(false);
        this.transform.root.GetComponent<MAMainMenu>().ResumeGame();
    }
}
