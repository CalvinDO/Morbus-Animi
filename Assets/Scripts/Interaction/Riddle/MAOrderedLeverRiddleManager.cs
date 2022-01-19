using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAOrderedLeverRiddleManager : MonoBehaviour {
    private MAOrderedLever[] riddleLevers;

    private int currentIndex = 0;

    private int maxIndex;

    public MADoor riddleDoor;

    void Start() {

        this.riddleLevers = this.transform.GetComponentsInChildren<MAOrderedLever>();

        this.maxIndex = this.riddleLevers.Length;

        for (int index = 0; index < this.maxIndex; index++) {
            this.riddleLevers[index].order = index;
        }
    }


    public void PressedLeverIndex(int index) {
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

        this.ResetLevers();

    }

    void WinRiddle() {
        Debug.LogWarning("!!!!riddle won!!!!");

        this.ResetLevers();

        this.riddleDoor.Open();
    }

    void ResetLevers() {
        this.currentIndex = 0;

        foreach (MAOrderedLever lever in this.riddleLevers) {
            lever.Reset();
        }
    }
}
