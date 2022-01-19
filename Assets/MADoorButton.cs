using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MADoorButton : MAButton
{

    public MADoor door;


    public override void SetPressed() {
        base.SetPressed();

        this.door.Open();
    }
}
