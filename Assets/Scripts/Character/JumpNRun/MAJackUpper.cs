using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAJackUpper : MonoBehaviour {
    public Transform handUpMaxPosition;
    public Transform handDownMinPosition;
    public float handUpMaxTolerance;

    public float maxLookAtAngle;


    public MAJackUpEnoughSpaceChecker enoughSpaceTrigger;

    public bool isHanging = false;
    public bool isCatpassing = false;


    private Vector3 currentClosestPoint;
    private Vector3 attachedPoint;


    private bool runAlgorithm = true;


    public float obstacleCollectorRadius;


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


    public Collider[] GetCurrentObstacles() {

        Collider[] physicOverlappedObstacles = Physics.OverlapSphere(this.attachedPoint, this.obstacleCollectorRadius);


        List<Collider> filteredColliders = new List<Collider>();

        foreach (Collider obstacle in physicOverlappedObstacles) {
            if (!obstacle.isTrigger) {
                if (!obstacle.transform.root.GetComponent<MACharacterController>()) {

                    filteredColliders.Add(obstacle);
                }
            }
        }

        return filteredColliders.ToArray();
    }


    private void OnTriggerStay(Collider ledgeCandidateCollider) {

        if (ledgeCandidateCollider.isTrigger) {
            return;
        }

        if (!this.runAlgorithm) {
            return;
        }



        Vector3 closestPoint = Vector3.zero;

        if (ledgeCandidateCollider.GetType() == typeof(BoxCollider) || ledgeCandidateCollider.GetType() == typeof(SphereCollider)) {
            closestPoint = ledgeCandidateCollider.ClosestPoint(this.handUpMaxPosition.position);
        }
        else if (ledgeCandidateCollider.GetType() == typeof(MeshCollider)) {

            MeshCollider currentMeshCollider = (MeshCollider)ledgeCandidateCollider;

            if (currentMeshCollider.convex) {
                closestPoint = ledgeCandidateCollider.ClosestPoint(this.handUpMaxPosition.position);
            }
        }

        if (closestPoint == Vector3.zero) {
            return;
        }

        this.currentClosestPoint = closestPoint;

        Vector3 sameYCharacterClosestPoint = this.transform.position;
        sameYCharacterClosestPoint.y = closestPoint.y;


        if (closestPoint.y > this.handDownMinPosition.position.y && closestPoint.y < this.handUpMaxPosition.position.y - this.handUpMaxTolerance) {

            if (this.isHanging || this.isCatpassing) {
                return;
            }

            if (Vector3.Angle(this.transform.forward, closestPoint - sameYCharacterClosestPoint) < this.maxLookAtAngle) {

                if (this.enoughSpaceTrigger.GetSpaceFreeAt(closestPoint)) {
                    Debug.DrawLine(this.handUpMaxPosition.position, closestPoint, Color.green);

                    this.DecideCatpassOrHang(closestPoint, sameYCharacterClosestPoint);
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


    private void OnDrawGizmos() {


        if (this.isHanging) {
            //Debug.Log("hang: " + this.isHanging + "  |||   catpass: " + this.isCatpassing);
            Gizmos.DrawWireSphere(this.attachedPoint, 0.2f);
            return;
        }

        if (this.isCatpassing) {
            // Debug.Log("hang: " + this.isHanging + "  |||   catpass: " + this.isCatpassing);

            Gizmos.DrawSphere(this.attachedPoint, 0.2f);
        }

    }

    private void DecideCatpassOrHang(Vector3 closestPoint, Vector3 sameYCharacterClosestPoint) {

        Ray floorRay = new Ray(sameYCharacterClosestPoint, Vector3.down);


        if (Physics.Raycast(floorRay, out _, this.handUpMaxPosition.localPosition.y, 13)) {

            this.Catpass(closestPoint);
        }
        else {

            this.Hang(closestPoint);
        }

        /*
        if (closestPoint.y > (this.handDownMinPosition.transform.position.y + this.handUpMaxPosition.transform.position.y) / 2) {

            this.Hang(closestPoint);
        }
        else {
            this.Catpass(closestPoint);
        }
        */
    }



    private void DiscardLedge() {

        this.isHanging = false;
        this.isCatpassing = false;
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
