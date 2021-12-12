using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum MAJackUpperVariation {
    Catpass, Hang
}

public class MAJackUpper : MonoBehaviour {
    public Transform handUpMaxPosition;
    public Transform handDownMinPosition;
    public float handUpMaxTolerance;

    public float minLookAtLedgeDot;

    private bool isLedgeInRange = false;
    private bool isSpaceFree = false;

    private bool isHanging = false;
    private bool isCatpassing = false;

    public MAJackUpEnoughSpaceTrigger enoughSpaceTrigger;


    private Vector3 focusedEdgePos;


    private MACharacterController characterController;

    private Vector3 currentAttachedEdgePos;
    private bool isAttached;

    void Start() {
        this.focusedEdgePos = Vector3.zero;

        this.characterController = this.transform.root.GetComponent<MACharacterController>();
    }


    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.Backspace)) {
            this.isAttached = false;
            this.DiscardLedge();
        }
    }

    private void OnTriggerStay(Collider ledgeCandidateCollider) {

        if (ledgeCandidateCollider.isTrigger) {
            return;
        }

        if (this.isAttached) {
            return;
        }


        Vector3 closestPoint = ledgeCandidateCollider.ClosestPoint(this.handUpMaxPosition.position);

        Vector3 sameYCharacterClosestPoint = this.transform.position;
        sameYCharacterClosestPoint.y = closestPoint.y;

       

        if (closestPoint.y > this.handDownMinPosition.position.y && closestPoint.y < this.handUpMaxPosition.position.y - this.handUpMaxTolerance) {

            if (Vector3.Angle(this.transform.forward, closestPoint - sameYCharacterClosestPoint) < this.minLookAtLedgeDot) {

                this.isLedgeInRange = true;

                this.SetEnoughSpaceTriggerPosition(closestPoint);

                if (this.isSpaceFree) {

                    this.DecideHangOrCatpass(closestPoint);

                }
                else {
                    Debug.DrawLine(this.handUpMaxPosition.position, closestPoint, Color.yellow);
                    this.DiscardLedge();
                }
            }
            else {
                Debug.DrawLine(this.handUpMaxPosition.position, closestPoint, Color.red);
                this.DiscardLedge();
            }
        }
    }


    private void DecideHangOrCatpass(Vector3 closestPoint) {

        Debug.DrawLine(this.handUpMaxPosition.position, closestPoint, Color.green);


        RaycastHit hit;

        Vector3 heightDetectorPosition = new Vector3(this.transform.position.x, closestPoint.y - this.handUpMaxPosition.localPosition.y, this.transform.position.z);
        Ray ray = new Ray(heightDetectorPosition, Vector3.down);

        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 0.1f, Color.blue);

        if (Physics.Raycast(ray, out hit, 0.1f)) {

            this.Catpass(closestPoint);

        }
        else {

            this.Hang(closestPoint);
        }

        if (this.characterController.isGrounded) {
            this.Catpass(closestPoint);
        }

        this.focusedEdgePos = closestPoint;
    }

    private void Hang(Vector3 closestPoint) {
        this.isHanging = true;
        this.isCatpassing = false;

        this.SetCurrentAttached(closestPoint);

    }

    private void Catpass(Vector3 closestPoint) {
        this.isHanging = false;
        this.isCatpassing = true;

        this.SetCurrentAttached(closestPoint);
    }

    private void SetCurrentAttached(Vector3 closestPoint) {
        this.currentAttachedEdgePos = closestPoint;
        this.isAttached = true;
    }


    private void OnDrawGizmos() {
        if (this.isCatpassing) {
            Gizmos.DrawSphere(this.currentAttachedEdgePos, 0.2f);
        }

        if (this.isHanging) {
            Gizmos.DrawWireSphere(this.currentAttachedEdgePos, 0.2f);
        }

    }

    private void SetEnoughSpaceTriggerPosition(Vector3 closestPoint) {
        this.enoughSpaceTrigger.transform.position = closestPoint + Vector3.up * 0.05f;
    }


    private void DiscardLedge() {
        this.isLedgeInRange = false;
        this.isHanging = false;
        this.isCatpassing = false;
        this.isSpaceFree = false;
    }


    public void SetSpaceFree() {
        this.isSpaceFree = true;
    }

    public void SetSpaceOccupied() {
        this.isSpaceFree = false;
    }
}
