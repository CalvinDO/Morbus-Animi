using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New ObjectivePath", menuName = "ObjectivePath")]
public class MAObjectivePath : ScriptableObject {

    
    [Tooltip("Place your Objectives:")]
    public string[] objectives;

    [Tooltip("Set State of each Objective:")]
    public Transform[] states;
}