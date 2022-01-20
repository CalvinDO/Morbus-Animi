using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAOrderedButton : MAButton {

    public int order;

    private MAOrderedButtonRiddleManager riddleManager;

    void Start() {
        this.riddleManager = this.transform.parent.GetComponent<MAOrderedButtonRiddleManager>();
    }


    public override void SetPressed() {

        base.SetPressed();

        this.riddleManager.PressedButtonIndex(this.order);
    }

    public override void Reset() {

        base.Reset();

        this.SetDefault();
    }
}
