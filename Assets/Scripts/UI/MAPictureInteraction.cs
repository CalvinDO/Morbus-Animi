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
    public MADarkNorah darkNorah;

    public void showNorah() {
        this.darkNorah.gameObject.SetActive(true);

        this.darkNorah.StartMoving();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<MACharacterController>() == null) {
            return;
        }

        this.MAInteract();
    }
    public override void MAInteract()
    {
        clearText();
        base.MAInteract();
        panel.GetComponent<Image>().sprite = picture.icon;
        text.text = picture.description;

        if (this.darkNorah != null) {
            canvas.GetComponentInChildren<Button>().onClick.AddListener(this.showNorah);
        }

        canvas.gameObject.SetActive(!canvas.gameObject.activeSelf);
        if (canvas.gameObject.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
