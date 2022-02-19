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

    public Image image;
    public RectTransform imageCollapsedTransform;
    public RectTransform imageDisplayedTransform;
    public float lerpFactor;

    private bool showDisplay = true;
    public float timeTillHideDisplay = 6;
    private float remainingTimeTillHideDisplay;

    public AudioSource audioSource;
    public AudioClip displayPopopSound;


    void Start() {
        instance.remainingTimeTillHideDisplay = instance.timeTillHideDisplay;
    }

    public void IncreaseObjectiveIndex() {
        instance.currentIndex += 1;

        instance.UpdateDisplay();


        instance.audioSource.PlayOneShot(instance.displayPopopSound);
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

        instance.remainingTimeTillHideDisplay = instance.timeTillHideDisplay;
        instance.ShowDisplay();
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.I)) {
            instance.IncreaseObjectiveIndex();
        }

        if (instance.showDisplay) {
            instance.remainingTimeTillHideDisplay -= Time.deltaTime;
        }
    }

    void FixedUpdate() {

        if (instance.showDisplay) {
            instance.ShowDisplay();


            if (instance.remainingTimeTillHideDisplay <= 0) {
                instance.HideDisplay();
            }
        }
        else {
            instance.HideDisplay();
        }
    }

    private void HideDisplay() {

        instance.showDisplay = false;
        instance.remainingTimeTillHideDisplay = instance.timeTillHideDisplay;

        instance.image.rectTransform.position = Vector3.Lerp(instance.image.rectTransform.position, instance.imageCollapsedTransform.position, instance.lerpFactor);
    }

    private void ShowDisplay() {

        instance.showDisplay = true;

        instance.image.rectTransform.position = Vector3.Lerp(instance.image.rectTransform.position, instance.imageDisplayedTransform.position, instance.lerpFactor);
    }
}
