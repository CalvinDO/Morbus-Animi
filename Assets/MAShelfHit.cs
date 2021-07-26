using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAShelfHit : MonoBehaviour {
    public AudioSource audioSource;
    public AudioClip shelfHit;
    public MAShelfTipper shelfTipper;

    // Start is called before the first frame update
    void Start() {

    }

    private void OnTriggerEnter(Collider other) {
        if (this.shelfTipper.isExploded) {
            this.audioSource.PlayOneShot(this.shelfHit);
        }
    }


    // Update is called once per frame
    void Update() {

    }
}
