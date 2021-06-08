using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAWinnerPathFabric : MAPathFabric
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update() {

        this.CalculateAngularVelocity();

        this.SetRandomTurnFrequency();

        this.SetRandomDirectionalWeight();

        if (!this.fatalCrashed) {
            if (!this.Move()) {
                this.fatalCrashed = true;
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
