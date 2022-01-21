using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MADoor : MonoBehaviour {
    public MAAxis axis;

    [Range(-360, 360)]
    public float openAngle;

    private bool isOpen = false;

    public MAItem requiredItem;


    public AudioSource doorAudioSource;
    public AudioClip doorFail;
    public AudioClip doorOpen;


    public void Open() {

        if (this.isOpen) {
            return;
        }

        if (this.requiredItem != null) {
            if (!MAInventory.instance.items.Contains(this.requiredItem)) {

                this.doorAudioSource.PlayOneShot(this.doorFail);

                return;
            }
        }

        this.doorAudioSource.PlayOneShot(this.doorOpen);

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
