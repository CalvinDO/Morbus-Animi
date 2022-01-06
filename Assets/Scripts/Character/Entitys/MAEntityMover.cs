using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MAEntityMover : MonoBehaviour {

    [Range(0, 1f)]
    public float defaultSpeed;

    [Range(0, 400f)]
    public float defaultAngularSpeed;

    [Range(0, 100f)]
    public float defaultAcceleration;


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

    public float maxSpeed;


    void Start() {
        this.currentDecisionTime = this.maxDecisionTime;

        this.navMeshCenter = GameObject.Find("NavMeshCenter").transform;


        this.SetNavAgentDefaultValues();
    }

    private void Update() {
        this.currentDecisionTime -= Time.deltaTime;

        if (this.currentDecisionTime <= 0) {
            this.SetNewRandomDestination();
        }

        if (this.frustumDetector.characterDetected) {
            this.navMeshAgent.SetDestination(this.frustumDetector.lastSeenCharacterPosition);
            this.IncreaseEntitySpeed();
            return;
        }


        if ((this.navMeshAgent.remainingDistance < this.destinationReachThreshhold) || this.navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid) {
            this.SetNewRandomDestination();
            this.currentDecisionTime = this.maxDecisionTime;
        }

    }

    private void IncreaseEntitySpeed() {
        if (this.navMeshAgent.speed > this.maxSpeed) {
            return;
        }

        this.navMeshAgent.speed += this.characterDetectedSpeedIncrease * Time.deltaTime;
        this.navMeshAgent.angularSpeed += this.characterDetectedSpeedIncrease * Time.deltaTime;
        this.navMeshAgent.acceleration += this.characterDetectedSpeedIncrease * Time.deltaTime;
    }

    public void LostCharacter() {
        this.SetNavAgentDefaultValues();
    }


    private void SetNavAgentDefaultValues() {
        this.navMeshAgent.speed = this.defaultSpeed;
        this.navMeshAgent.angularSpeed = this.defaultAngularSpeed;
        this.navMeshAgent.acceleration = this.defaultAcceleration;

    }



    private void SetNewRandomDestination() {

        this.randomPointInSphere = Random.insideUnitSphere * this.randomDestinationRange + this.transform.parent.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(this.randomPointInSphere, out hit, int.MaxValue, 1);

        this.navMeshAgent.SetDestination(hit.position);


        float newDecisionTime = Random.Range(this.minDecisionTime, this.maxDecisionTime);
        this.currentDecisionTime = newDecisionTime;

    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(this.transform.parent.position, this.randomDestinationRange);
        // Gizmos.DrawWireSphere(this.randomPointInSphere, 5);
        Gizmos.DrawSphere(this.navMeshAgent.destination, 1);
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
