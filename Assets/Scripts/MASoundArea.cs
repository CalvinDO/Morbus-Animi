using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MASoundArea : MonoBehaviour
{
    public AudioSource audioSource;
        
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<MACharacterController>() != null){
            this.audioSource.PlayOneShot(this.audioSource.clip);
        }
    }
}
