using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAOrderedLever : MALever {

    public int order;

    private MAOrderedLeverRiddleManager riddleManager;



    void Start() {
        this.riddleManager = this.transform.parent.GetComponent<MAOrderedLeverRiddleManager>();
    }


    public override void SetPressed() {

        base.SetPressed();

        this.riddleManager.PressedLeverIndex(this.order);
    }

    public override void Reset() {

        if (!this.isPressed) {
            return;
        }

        this.SetDefault();
        
    }
}
