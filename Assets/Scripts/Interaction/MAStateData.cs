using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MAStateData : ScriptableObject
{
    public float Duration;

    public abstract void OnEnter(MACharacterState characterState, Animator animator, AnimatorStateInfo stateInfo);
    public abstract void UpdateAbility(MACharacterState characterState, Animator animator, AnimatorStateInfo stateInfo);
    public abstract void OnExit(MACharacterState characterState, Animator animator, AnimatorStateInfo stateInfo);
}
