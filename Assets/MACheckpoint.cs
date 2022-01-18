using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MACheckpoint : MonoBehaviour {


   
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider other) {
        this.transform.parent.GetComponent<MACheckpointManager>().currentCheckpoint = this.transform;
    }
}
