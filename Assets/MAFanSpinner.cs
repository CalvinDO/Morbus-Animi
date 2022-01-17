using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MAAxis {
    X, Y, Z
}

public class MAFanSpinner : MonoBehaviour {

    [Range(0, 500)]
    public float spinSpeed;
    public MAAxis axis;
    public bool isSpinning = true;
    public WindZone windZone;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (!this.isSpinning) {
            return;
        }

        Vector3 axisVector = Vector3.up;

        switch (this.axis) {
            case MAAxis.X:

                axisVector = Vector3.right;
                break;
            case MAAxis.Y:
                axisVector = Vector3.up;
                break;
            case MAAxis.Z:
                axisVector = Vector3.forward;
                break;
        }


        this.transform.Rotate(axisVector * this.spinSpeed * Time.deltaTime);
    }


    public void Stop() {
        this.isSpinning = false;
        this.windZone.windMain = 0;
    }
}
