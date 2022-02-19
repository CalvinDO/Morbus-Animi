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


    public bool isPressed = false;


    public AudioSource audioSource;
    public AudioClip pressSound;


    public MACharacterController characterController;


    public MAIndicatorLookat indicator;

    void Awake() {
        this.characterController = GameObject.FindObjectOfType<MACharacterController>().GetComponent<MACharacterController>();
    }

    void Update() {

        if (this.indicator != null) {

            if (this.characterInReach && !this.isPressed) {

                this.indicator.gameObject.SetActive(true);
            }
            else {
                this.indicator.gameObject.SetActive(false);
            }
        }



        if (this.isPressed && this.staysPressed) {
            return;
        }


    

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


        Debug.Log("set state: " + value);
        this.characterController.animator.SetTrigger("interact");
    }

    public virtual void SetPressed() {

        Debug.Log("executing setPressed in MABUtton");

        this.isPressed = true;

        Material[] mats = new Material[] { this.buttonRenderer.materials[0], this.pressedMaterial };

        this.buttonRenderer.materials = mats;



        if (this.audioSource == null) {
            return;
        }

        if (this.pressSound == null) {
            return;
        }

        this.audioSource.PlayOneShot(this.pressSound);
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
