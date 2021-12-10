using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAJackUpper : MonoBehaviour {
    public Transform handUpMaxPosition;
    public Transform handDownMinPosition;
    public float handUpMaxTolerance;

    public float minLookAtLedgeDot;

    private bool isLedgeInRange = false;
    private bool isLedgeFree = false;



    public MAJackUpEnoughSpaceTrigger enoughSpaceTrigger;
      
    public bool isSpaceFree = false;
    

    void Start() {

    }


    // Update is called once per frame
    void Update() {
        //  Vector3 closestPoint = 
    }

    private void OnTriggerStay(Collider ledgeCandidateCollider) {
        if (ledgeCandidateCollider.isTrigger) {
            return;
        }

        Vector3 closestPoint = ledgeCandidateCollider.ClosestPoint(this.handUpMaxPosition.position);

        Vector3 sameYCharacterClosestPoint = this.transform.position;
        sameYCharacterClosestPoint.y = closestPoint.y;

        if (closestPoint.y > this.handDownMinPosition.position.y && closestPoint.y < this.handUpMaxPosition.position.y - this.handUpMaxTolerance) {

            Debug.DrawLine(this.transform.position, this.transform.position + closestPoint - sameYCharacterClosestPoint);

            Debug.Log(Vector3.Angle(this.transform.forward, closestPoint - sameYCharacterClosestPoint));

            if (Vector3.Angle(this.transform.forward, closestPoint - sameYCharacterClosestPoint) < this.minLookAtLedgeDot) {
               
                this.isLedgeInRange = true;

                this.SetEnoughSpaceTriggerPosition(closestPoint);

                if (this.isSpaceFree) {
                    Debug.DrawLine(this.handUpMaxPosition.position, closestPoint, Color.green);
                }

                Debug.DrawLine(this.handUpMaxPosition.position, closestPoint, Color.yellow);
            }
            else {
                Debug.DrawLine(this.handUpMaxPosition.position, closestPoint, Color.red);
                this.DiscardLedge();
            }
        }
    }

    private void SetEnoughSpaceTriggerPosition(Vector3 closestPoint) {
        this.enoughSpaceTrigger.transform.position = closestPoint + Vector3.up * 0.05f;
    }


    private void DiscardLedge() {
        this.isLedgeFree = false;
        this.isLedgeInRange = false;
    }


    public void SetSpaceFree() {
        this.isSpaceFree = true;
    }

    public void SetSpaceOccupied() {
        this.isSpaceFree = false;
    }
}
