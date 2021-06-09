using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAWinnerPathFabric : MAPathFabric {
    public MAPathFabric standardPathFabric;

    void Start() {

    }

    void Update() {

        this.CalculateAngularVelocity();

        this.SetRandomTurnFrequency();

        if (!this.fatalCrashed) {
            if (!this.Move()) {
                this.fatalCrashed = true;
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
