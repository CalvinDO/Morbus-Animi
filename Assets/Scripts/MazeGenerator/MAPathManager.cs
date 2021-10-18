using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAPathManager : MonoBehaviour {

    public static MAPathFabricDirection directionalWeightStatic;
    public MAPathFabricDirection directionalWeight;

    public int framesTillDirectionalWeightChange;
    private float framesSinceLastDirectionalWeightChange;

    [Range(0, 1)]
    public float directionalWeightStrength;
    public static float directionalWeightStrengthStatic;

    void Start() {
        directionalWeightStatic = directionalWeight;
        directionalWeightStrengthStatic = this.directionalWeightStrength;
    }

    // Update is called once per frame
    void Update() {
        this.SetRandomDirectionalWeight();
        this.directionalWeight = directionalWeightStatic;
    }

    public void SetRandomDirectionalWeight() {
        if (this.framesSinceLastDirectionalWeightChange > this.framesTillDirectionalWeightChange) {
            this.framesSinceLastDirectionalWeightChange = 0;
            this.ChangeRandomDirectionalWeight();

            directionalWeightStatic = MAPathFabric.ModuloDirection(directionalWeightStatic);
        }
        else {
            this.framesSinceLastDirectionalWeightChange++;
        }
    }

    private void ChangeRandomDirectionalWeight() {
        directionalWeightStatic -= MAPathFabric.GetRandomDirectionChange();

    }

}
