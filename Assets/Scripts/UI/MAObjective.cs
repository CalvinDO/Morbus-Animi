using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Objective", menuName = "Objective")]
public class MAObjective : ScriptableObject {
    
    [Multiline]
    [Tooltip("Write your new Objective:")]
    public string objective;
}
