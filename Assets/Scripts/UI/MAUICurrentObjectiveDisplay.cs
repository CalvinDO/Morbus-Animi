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


    #region Singleton
    public static MAUICurrentObjectiveDisplay instance;
    void Awake() {
        if (instance == null) {
            instance = this;
        }

        instance.UpdateDisplay();
    }
    #endregion

    private int currentIndex = 0;

    public Text displayText;

    public MAObjectivePath objectivePath;

    [Tooltip("Place your Objectives:")]
    public string[] objectives;

    [Tooltip("Set State of each Objective:")]
    public Transform[] indicatorLocations;

    public GameObject objectiveIndicator;

    public void IncreaseObjectiveIndex() {
        instance.currentIndex += 1;

        instance.UpdateDisplay();
    }

    private void UpdateDisplay() {

        string objective = instance.objectives[instance.currentIndex];
        instance.displayText.text = objective;

        if (instance.indicatorLocations[instance.currentIndex] != null) {
            instance.objectiveIndicator.SetActive(true);
            instance.objectiveIndicator.transform.position = instance.indicatorLocations[instance.currentIndex].position;
            return;
        }

        instance.objectiveIndicator.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.I)) {
            instance.IncreaseObjectiveIndex();
        }
    }
}
