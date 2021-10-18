using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MANorahsFatherMover : MonoBehaviour {

    [Range(0, 0.03f)]
    public float speed;

    public Transform firstGoal;
    public Transform secondGoal;
    public float goalReachThreshhold;

    public float minDecisionTime;
    public float maxDecisionTime;

    private float currentDecisionTime;

    private Transform currentGoal;

    void Start() {
        this.currentGoal = this.GetRandomBool();
        this.currentDecisionTime = this.maxDecisionTime;
    }


    void FixedUpdate() {
        this.Move();
    }

    private void SwitchDirection() {

        this.currentGoal = this.currentGoal.Equals(this.firstGoal) ? this.secondGoal : this.firstGoal;

        float newDecisionTime = Random.Range(this.minDecisionTime, this.maxDecisionTime);
        this.currentDecisionTime = newDecisionTime;
        Debug.Log("switched direction to: " + this.currentGoal.position);
    }

    private void Update() {
        this.currentDecisionTime -= Time.deltaTime;

        if (this.currentDecisionTime <= 0) {
            this.SwitchDirection();
        }
    }

    private void Move() {
        Vector3 connection = this.currentGoal.position - this.transform.position;
        if (connection.magnitude< this.goalReachThreshhold) {
            return;
        }

        Vector3 connectionDirection = connection.normalized;
        Vector3 increment = connectionDirection * this.speed;

        this.transform.LookAt(this.currentGoal.position);
        this.transform.Translate(increment, Space.World);
    }

    private Transform GetRandomBool() {
        Debug.Log(Random.Range(0, 2));
        return Random.Range(0, 2) == 0 ? this.firstGoal : this.secondGoal;
    }
}
