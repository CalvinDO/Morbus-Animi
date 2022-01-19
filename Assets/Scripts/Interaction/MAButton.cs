using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class MAButton : MonoBehaviour {

    public bool staysPressed = false;
    public Material defaultMaterial;
    public Material pressedMaterial;
    public MeshRenderer buttonRenderer;


    private bool characterInReach = false;

    [HideInInspector]
    public bool isPressed = false;


    void Update() {

        if (this.characterInReach) {

            if (Input.GetKeyUp(KeyCode.E)) {

                Debug.Log("press press");
                if (!this.staysPressed) {

                    this.isPressed = !this.isPressed;
                    
                }
                else {
                    this.isPressed = true;
                }

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

    public void SetState(bool value) {
        if (value) {
            this.SetPressed();
        }
        else {
            this.SetDefault();
        }
    }

    public virtual void SetPressed() {

        this.isPressed = true;

        Material[] mats = new Material[] { this.buttonRenderer.materials[0], this.pressedMaterial };

        this.buttonRenderer.materials = mats;
        
    }

    public virtual void SetDefault() {

        Debug.Log("default");

        this.isPressed = false;

   
        this.buttonRenderer.material = this.defaultMaterial;
    }

    public virtual void Reset() {

        this.SetDefault();
    }

}
