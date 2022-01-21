using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAFlashlight : MonoBehaviour
{
    public Light flashlightLight;
    public GameObject lightEmissiveObject;

    public AudioSource audioSource;
    public AudioClip turnOnSound;
    public AudioClip turnOffSound;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F)) {
            this.flashlightLight.enabled = !this.flashlightLight.enabled;
            this.lightEmissiveObject.SetActive(this.lightEmissiveObject.activeSelf);



            if (this.flashlightLight.enabled) {
                this.audioSource.PlayOneShot(this.turnOnSound);
            }
            else {
                this.audioSource.PlayOneShot(this.turnOffSound);
            }
        }
    }
}
