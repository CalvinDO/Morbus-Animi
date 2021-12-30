using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MARiddleDoor : MonoBehaviour {

    [Range(0, 360)]
    public float angle;

    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Open() {
        this.transform.Rotate(new Vector3(0, this.angle));

    }

    public void Close() {
        this.transform.Rotate(new Vector3(0, -this.angle));

    }
}
