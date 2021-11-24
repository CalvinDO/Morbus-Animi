using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MAUIPicturePrefab : MonoBehaviour
{
    public Canvas canvas;
    public Image panel;
    public Text text;

    public MAMainMenu mainMenu;

    public void showPicture(MAPicture picture)
    {
        this.panel.GetComponent<Image>().sprite = picture.icon;
        this.text.text = picture.description;


        this.canvas.gameObject.SetActive(!this.canvas.gameObject.activeSelf);

        mainMenu.PauseGame();

        return;
    }
}
