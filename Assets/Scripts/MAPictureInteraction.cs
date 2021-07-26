using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MAPictureInteraction : MAInteractable
{
    public MAItem picture;
    public GameObject canvas;
    public GameObject panel;
    public Text text;
    public override void MAInteract()
    {
        clearText();
        base.MAInteract();
        panel.GetComponent<Image>().sprite = picture.icon;
        text.text = picture.description;
        canvas.SetActive(!canvas.activeSelf);
        if (canvas.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
