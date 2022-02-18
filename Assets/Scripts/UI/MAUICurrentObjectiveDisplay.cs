using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
[CreateAssetMenu(fileName = "New Objective", menuName = "Objective")]
public class MAObjective : ScriptableObject {

    [Multiline]
    [Tooltip("Write your new Objective")]
    new public string name = "New Objective";
}
*/

public class MAUICurrentObjectiveDisplay : MonoBehaviour {

    private int currentIndex = 0;

    public Text displayText;

    public MAObjectivePath objectivePath;

    [Tooltip("Place your Objectives:")]
    public string[] objectives;

    [Tooltip("Set State of each Objective:")]
    public Transform[] indicatorLocations;

    public GameObject objectiveIndicator;

    void Start() {
        this.UpdateDisplay();
    }

    public void IncreaseObjectiveIndex() {
        this.currentIndex += 1;

        this.UpdateDisplay();
    }

    private void UpdateDisplay() {

        string objective = this.objectives[this.currentIndex];
        this.displayText.text = objective;

        if (this.indicatorLocations[this.currentIndex] != null) {
            this.objectiveIndicator.SetActive(true);
            this.objectiveIndicator.transform.position = this.indicatorLocations[this.currentIndex].position;
            return;
        }

        this.objectiveIndicator.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.I)) {
            this.IncreaseObjectiveIndex();
        }
    }
}
