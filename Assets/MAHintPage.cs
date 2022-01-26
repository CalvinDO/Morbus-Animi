using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAHintPage : MAPictureInteraction {

    public static bool isCurrentlyOpened = false;



    public override void Update() {

        if (MAHintPage.isCurrentlyOpened) {

            if (Input.GetKeyUp(KeyCode.E) && this.isCharacterInTrigger) {

                this.CloseHintPage();

                return;
            }
        }

        base.Update();
    }

    private void CloseHintPage() {
        this.uiPicturePrefab.Close();
        MAHintPage.isCurrentlyOpened = false;

        
    }

    public override void DoCollectActions() {

        MAHintPage.isCurrentlyOpened = true;
    }

}
