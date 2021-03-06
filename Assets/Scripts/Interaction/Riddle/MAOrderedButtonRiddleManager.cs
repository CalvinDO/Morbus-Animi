using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAOrderedButtonRiddleManager : MonoBehaviour {
    private MAOrderedButton[] riddleButtons;

    private int currentIndex = 0;

    private int maxIndex;

    public MADoor riddleDoor;

    void Start() {
        this.riddleButtons = this.transform.GetComponentsInChildren<MAOrderedButton>();

        this.maxIndex = this.riddleButtons.Length;

        for (int index = 0; index < this.maxIndex; index++) {
            this.riddleButtons[index].order = index;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void PressedButtonIndex(int index) {
        if (index != this.currentIndex) {
            this.FailRiddle();
        }

        if (index == this.currentIndex) {
            this.currentIndex++;
        }


        if (this.currentIndex > this.maxIndex) {
            this.WinRiddle();
        }
    }

    void FailRiddle() {
        Debug.LogWarning("!!!!riddle failed!!!!");

        this.ResetButtons();

       // this.riddleDoor.Close();
    }

    void WinRiddle() {
        Debug.LogWarning("!!!!riddle won!!!!");

        this.ResetButtons();

        this.riddleDoor.Open();
    }

    void ResetButtons() {
        this.currentIndex = 0;

        foreach (MAOrderedButton button in this.riddleButtons) {
            button.Reset();
        }
    }
}
