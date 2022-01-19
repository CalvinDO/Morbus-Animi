using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MALever : MAButton {
    public MAAxis axis;

    [Range(-360, 360)]
    public float openAngle;

    public Transform rotatingLever;



    public override void SetPressed() {

        Debug.Log("press");

        this.isPressed = true;

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

        this.rotatingLever.transform.Rotate(axisVector * this.openAngle);
    }


    public override void SetDefault() {

        Debug.Log("default");

        if (this.staysPressed) {
            return;
        }

        this.isPressed = false;

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

        this.rotatingLever.transform.Rotate(axisVector * -this.openAngle);
    }
}
