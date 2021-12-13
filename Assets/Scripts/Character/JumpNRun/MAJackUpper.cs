using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAJackUpper : MonoBehaviour {
    public Transform handUpMaxPosition;
    public Transform handDownMinPosition;
    public float handUpMaxTolerance;

    public float maxLookAtAngle;

    private bool isLedgeInRange = false;
    private bool isLedgeFree = false;



    public MAJackUpEnoughSpaceTrigger enoughSpaceTrigger;

    public bool isSpaceFree = true;

    public bool isHanging = false;
    public bool isCatpassing = false;


    private Vector3 attachedPoint;


    void Start() {

    }


    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.Backspace)) {
            this.DiscardLedge();
            this.isHanging = false;
            this.isCatpassing = false;
        }
    }

    private void OnTriggerStay(Collider ledgeCandidateCollider) {
        if (ledgeCandidateCollider.isTrigger) {
            return;
        }

        Vector3 closestPoint = ledgeCandidateCollider.ClosestPoint(this.handUpMaxPosition.position);

        Vector3 sameYCharacterClosestPoint = this.transform.position;
        sameYCharacterClosestPoint.y = closestPoint.y;




        if (closestPoint.y > this.handDownMinPosition.position.y && closestPoint.y < this.handUpMaxPosition.position.y - this.handUpMaxTolerance) {

            //Debug.DrawLine(this.transform.position, this.transform.position + closestPoint - sameYCharacterClosestPoint, Color.yellow);
            // Debug.DrawRay(this.transform.position, this.transform.forward, Color.cyan);

            //Debug.Log(Vector3.Angle(this.transform.forward, closestPoint - sameYCharacterClosestPoint));

            if (this.isHanging || this.isCatpassing) {
                return;
            }

            if (Vector3.Angle(this.transform.forward, closestPoint - sameYCharacterClosestPoint) < this.maxLookAtAngle) {

                this.isLedgeInRange = true;

                this.SetEnoughSpaceTriggerPosition(closestPoint);

                if (this.isSpaceFree) {
                    Debug.DrawLine(this.handUpMaxPosition.position, closestPoint, Color.green);

                    this.DecideCatpassOrHang(closestPoint);
                }
                else {
                    Debug.DrawLine(this.handUpMaxPosition.position, closestPoint, Color.yellow);
                }

            }
            else {
                Debug.DrawLine(this.handUpMaxPosition.position, closestPoint, Color.red);
                this.DiscardLedge();
            }
        }
    }

    private void OnDrawGizmos() {

        if (this.isHanging) {
            Gizmos.DrawWireSphere(this.attachedPoint, 0.2f);
            return;
        }

        if (this.isCatpassing) {
            Gizmos.DrawSphere(this.attachedPoint, 0.2f);
        }
    }

    private void DecideCatpassOrHang(Vector3 closestPoint) {
        if (closestPoint.y > (this.handDownMinPosition.transform.position.y + this.handUpMaxPosition.transform.position.y) / 2) {
            this.Hang(closestPoint);
        }
        else {
            this.Catpass(closestPoint);
        }
    }

    private void SetEnoughSpaceTriggerPosition(Vector3 closestPoint) {
        this.isSpaceFree = false;
        this.enoughSpaceTrigger.transform.position = closestPoint + Vector3.up * 0.05f;
    }


    private void DiscardLedge() {
        this.isLedgeFree = false;
        this.isLedgeInRange = false;
        this.isSpaceFree = false;

        this.isHanging = false;
        this.isCatpassing = false;
    }


    public void SetSpaceFree() {
        this.isSpaceFree = true;
    }

    public void SetSpaceOccupied() {
        this.isSpaceFree = false;
    }

    public void Hang(Vector3 closestPoint) {
        this.isHanging = true;
        this.isCatpassing = false;

        this.attachedPoint = closestPoint;
    }

    public void Catpass(Vector3 closestPoint) {
        this.isHanging = false;
        this.isCatpassing = true;

        this.attachedPoint = closestPoint;

    }
}
