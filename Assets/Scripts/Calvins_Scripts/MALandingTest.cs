using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MALandingTest : MonoBehaviour {
    // Start is called before the first frame update


    public Rigidbody rb;

    void Start() {
        this.rb.velocity = new Vector3(0, 5, 3);
    }

    // Update is called once per frame
    void Update() {

    }
}
