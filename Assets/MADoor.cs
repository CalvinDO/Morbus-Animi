using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MADoor : MonoBehaviour {
    public MAAxis axis;

    [Range(-360, 360)]
    public float openAngle;

    private bool isOpen = false;

    public MAItem requiredItem;


    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Open() {

        if (this.isOpen) {
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


        this.transform.Rotate(axisVector * this.openAngle);

        this.isOpen = true;
    }
}
