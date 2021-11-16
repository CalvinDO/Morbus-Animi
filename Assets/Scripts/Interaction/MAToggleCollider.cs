using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "Assets/Scripts/Interaction/MAToggleCollider")]
public class MAToggleCollider : MAStateData
{
    public bool OnStart;
    public bool OnEnd;
    public bool On;

    public override void OnEnter(MACharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        if (OnStart)
        {
            MACharacterController controller = characterState.GetCharacterControl(animator);
            ToggleCol(controller);
        }
    }

    public override void UpdateAbility(MACharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {

    }

    public override void OnExit(MACharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        if (OnEnd)
        {
            MACharacterController controller = characterState.GetCharacterControl(animator);
            ToggleCol(controller);
        }
    }

    private void ToggleCol(MACharacterController controller)
    {
        controller.GetComponent<Rigidbody>().velocity = Vector3.zero;
        controller.GetComponentInChildren<CapsuleCollider>().enabled = On;
    }
}
