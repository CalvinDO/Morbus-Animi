using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MALever : MAButton {
    public MAAxis axis;

    [Range(-360, 360)]
    public float openAngle;

    public Transform rotatingLever;

    public AudioClip resetSound;

    public override void SetPressed() {

        Debug.Log("executing setPressed in MALever");


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
            default:
                break;
        }


        Debug.Log("rotate " + this.openAngle + " dregrees");

        this.rotatingLever.transform.Rotate(axisVector * this.openAngle);


        if (this.audioSource == null) {
            return;
        }

        if (this.pressSound == null) {
            return;
        }

        this.audioSource.PlayOneShot(this.pressSound);
    }


    public override void SetDefault() {

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




        if (this.resetSound == null) {
            return;
        }

        if (this.audioSource == null) {
            return;
        }

        this.audioSource.Stop();
        this.audioSource.PlayOneShot(this.resetSound);
    }
}
