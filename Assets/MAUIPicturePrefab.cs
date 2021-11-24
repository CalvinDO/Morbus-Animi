using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MAUIPicturePrefab : MonoBehaviour
{
    public Canvas canvas;
    public Image panel;
    public Text text;


    public void showPicture(MAPicture picture)
    {
        this.panel.GetComponent<Image>().sprite = picture.icon;
        this.text.text = picture.description;


        this.canvas.gameObject.SetActive(!this.canvas.gameObject.activeSelf);
        if (this.canvas.gameObject.activeSelf) {
            Time.timeScale = 0;
        }
        else {
            Time.timeScale = 1;
        }

        Cursor.lockState = CursorLockMode.None;
    }
}
