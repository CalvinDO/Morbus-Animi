using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAJackUpEnoughSpaceChecker : MonoBehaviour {

    public MAJackUpper jackUpper;

    private Transform[] spaceCheckPoints;


    void Start() {

        List<Transform> spaceCheckPointsList = new List<Transform>();


        foreach (Transform child in transform) {
            if (child.childCount > 0) {
                foreach (Transform secondIterationChild in child.transform) {
                    if (child.childCount > 0) {
                        foreach (Transform thirdIterationChild in secondIterationChild.transform) {
                            spaceCheckPointsList.Add(thirdIterationChild);
                        }
                    }
                }
            }
        }


        this.spaceCheckPoints = spaceCheckPointsList.ToArray();
    }

    private void OnDrawGizmos() {

        if (this.spaceCheckPoints == null) {
            return;
        }
        foreach (Transform spaceCheckPoint in this.spaceCheckPoints) {
            Gizmos.DrawWireSphere(spaceCheckPoint.position, 0.03f);
        }

    }

    public bool GetSpaceFreeAt(Vector3 closestPoint) {

        this.transform.position = closestPoint + Vector3.up * 0.05f;

        bool output = true;

        Collider[] currentObstacles = this.jackUpper.GetCurrentObstacles();


        foreach (Transform spaceCheckPoint in this.spaceCheckPoints) {

            foreach (Collider obstacle in currentObstacles) {

                Vector3 checkPositionLifted = spaceCheckPoint.position + Vector3.up * 0.05f;


                if (Physics.CheckSphere(checkPositionLifted, 0.0003f, 13, QueryTriggerInteraction.Ignore)) {
                    output = false;
                }

                /*

                if (obstacle.bounds.Contains(spaceCheckPoint.position + checkPositionLifted)) {
dw                    Debug.Log(obstacle.gameObject.name);
                }
                */
            }
        }


        return output;
    }
}
