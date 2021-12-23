using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAOrderedButton : MonoBehaviour {
    public int order;

    public MeshRenderer buttonRenderer;

    public Material defaultMaterial;
    public Material pressedMaterial;


    private bool isPressed = false;

    private bool characterInReach = false;

    private MAOrderedButtonRiddleManager riddleManager;

    void Start() {
        this.riddleManager = this.transform.parent.GetComponent<MAOrderedButtonRiddleManager>();
    }

    // Update is called once per frame
    void Update() {

        if (this.isPressed) {
            return;
        }

        if (this.characterInReach) {

            if (Input.GetKeyUp(KeyCode.E)) {
                // this.isPressed = !this.isPressed;
                this.isPressed = true;
                this.SetState(this.isPressed);
            }
        }
    }

    void OnTriggerStay(Collider characterCollider) {
        this.characterInReach = true;
    }

    void OnTriggerExit(Collider characterCollider) {
        this.characterInReach = false;
    }

    void SetState(bool value) {
        if (value) {
            this.SetPressed();
        }
        else {
            this.SetDefault();
        }
    }

    void SetPressed() {

        this.isPressed = true;

        this.buttonRenderer.material = this.pressedMaterial;

        this.riddleManager.PressedButtonIndex(this.order);
    }

    void SetDefault() {
        this.isPressed = false;
        this.buttonRenderer.material = this.defaultMaterial;
    }

    public void Reset() {

        Debug.Log($"reset button nr.:{this.order}");

        this.SetDefault();
    }
}
