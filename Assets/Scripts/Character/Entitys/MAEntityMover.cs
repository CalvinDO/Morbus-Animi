using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MAEntityMover : MonoBehaviour {

    [Range(0, 0.1f)]
    public float speed;

    [Range(0, 1f)]
    public float destinationReachThreshhold;

    public float minDecisionTime;
    public float maxDecisionTime;

    private float currentDecisionTime;

    public NavMeshAgent navMeshAgent;
    public MAFrustumDetector frustumDetector;

    public float randomDestinationRange;

    private Transform navMeshCenter;

    private Vector3 randomPointInSphere;

    public float characterDetectedSpeedIncrease;


    void Start() {
        this.currentDecisionTime = this.maxDecisionTime;

        this.navMeshCenter = GameObject.Find("NavMeshCenter").transform;
    }

    private void Update() {
        this.currentDecisionTime -= Time.deltaTime;

        if (this.currentDecisionTime <= 0) {
            this.SetNewRandomDestination();
        }

        if (this.frustumDetector.characterDetected) {
            this.navMeshAgent.SetDestination(this.frustumDetector.lastSeenCharacterPosition);
            this.navMeshAgent.speed += this.characterDetectedSpeedIncrease;
            return;
        }

   
        if ((this.navMeshAgent.remainingDistance < this.destinationReachThreshhold) || this.navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid) {
            this.SetNewRandomDestination();
            this.currentDecisionTime = this.maxDecisionTime;
        }

    }


    private void SetNewRandomDestination() {

        this.randomPointInSphere = Random.insideUnitSphere * this.randomDestinationRange + this.navMeshCenter.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(this.randomPointInSphere, out hit, int.MaxValue, 1);

        this.navMeshAgent.SetDestination(hit.position);


        float newDecisionTime = Random.Range(this.minDecisionTime, this.maxDecisionTime);
        this.currentDecisionTime = newDecisionTime;

    }

    private void OnDrawGizmos() {
        // Gizmos.DrawWireSphere(this.randomPointInSphere, 5);
        Gizmos.DrawSphere(this.navMeshAgent.destination, 4);
    }

    [System.Obsolete("Not needed anymore because NavMesh.SetDestination() is used")]
    private void Move() {
        /*
        Vector3 connection = this.currentGoal.position - this.transform.position;
        if (connection.magnitude< this.goalReachThreshhold) {
            return;
        }

        Vector3 connectionDirection = connection.normalized;
        Vector3 increment = connectionDirection * this.speed;

        this.transform.LookAt(this.currentGoal.position);
        this.navMeshAgent.Warp(this.transform.position + increment);
        */
    }
}
