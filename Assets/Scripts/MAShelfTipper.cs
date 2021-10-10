using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAShelfTipper : MonoBehaviour {
    public Rigidbody rigidbody;
    public Transform explosionPosition;
    public float explosionForce;
    public float explosionRadius;

    public AudioSource audioSource;
    public AudioClip shelfExplosion;

    public bool isExploded = false;

    void Start() { 

      
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.GetComponent<MACharacterController>() || this.isExploded) {
            return;
        }
        this.rigidbody.AddExplosionForce(this.explosionForce, this.explosionPosition.position, this.explosionRadius);
        this.audioSource.PlayOneShot(this.shelfExplosion);

        this.isExploded = true;
    }

    // Update is called once per frame
    void Update() {

    }
}
