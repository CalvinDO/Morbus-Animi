using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAHintPage : MAPictureInteraction
{

    private bool isCurrentlyOpened = false;


    public override void Update() {

        if (this.isCurrentlyOpened) {
            if (Input.GetKeyUp(KeyCode.E) && this.isCharacterInTrigger) {
                Debug.Log("e while opened!");
                return;
            }
        }

        base.Update();
    }

    public override void DoCollectActions() {

        this.isCurrentlyOpened = true;
        //do nothing, because a HintPage is not a diaryPage
    }

}
