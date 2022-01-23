using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class MAInventoryController : MonoBehaviour {



    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (!Input.anyKeyDown) {
            return;
        }

        string inputString = Input.inputString;

 
        try {
            if (int.Parse(inputString) < 0 || int.Parse(inputString) > 9) {
                throw new System.Exception();
            }
        }
        catch {
            return;
        }


        this.OpenPicture(int.Parse(inputString));
    }

    private void OpenPicture(int index) {

        MAUIPicturePrefab uiPicturePrefab = GameObject.Find("PictureUIPrefab").GetComponent<MAUIPicturePrefab>();
        try {
        uiPicturePrefab.ShowPicture(MAInventory.instance.pictures[index]);

        }
        catch {
            Debug.Log("slot " + index + " contains no image!");
            return;
        }
    }
}
