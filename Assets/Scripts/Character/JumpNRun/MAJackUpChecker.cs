using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAJackUpChecker : MonoBehaviour {
    public Transform handUpMaxPosition;
    public Transform handDownMinPosition;

    void Start() {

    }


    // Update is called once per frame
    void Update() {
        //  Vector3 closestPoint = 
    }

    private void OnTriggerStay(Collider other) {
        if (other.isTrigger) {
            return;
        }

        Vector3 closestPoint = other.ClosestPoint(this.handUpMaxPosition.position);


        if (closestPoint.y > this.handDownMinPosition.position.y && closestPoint.y < this.handUpMaxPosition.position.y) {


            if (Vector3.Dot(this.transform.forward, closestPoint - this.handUpMaxPosition.position) > 0) {
                Debug.DrawLine(this.handUpMaxPosition.position, closestPoint, Color.green);

            }
            else {
                Debug.DrawLine(this.handUpMaxPosition.position, closestPoint, Color.yellow);

            }
        }
        else {
            Debug.DrawLine(this.handUpMaxPosition.position, closestPoint, Color.red);
        }
    }
}
