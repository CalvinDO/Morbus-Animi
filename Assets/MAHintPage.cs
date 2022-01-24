using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAHintPage : MAPictureInteraction {

    private bool isCurrentlyOpened = false;


    public override void Update() {

        if (this.isCurrentlyOpened) {

            if (Input.GetKeyUp(KeyCode.E) && this.isCharacterInTrigger) {

                this.CloseHintPage();

                return;
            }
        }

        base.Update();
    }

    private void CloseHintPage() {
        this.uiPicturePrefab.Close();
        this.isCurrentlyOpened = false;
    }

    public override void DoCollectActions() {

        this.isCurrentlyOpened = true;
    }

}