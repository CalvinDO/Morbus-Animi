using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "Assets/Scripts/Interaction/MAOffsetOnLedge")]
public class MAOffsetOnLedge : MAStateData
{
    public override void OnEnter(MACharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        MACharacterController controller = characterState.GetCharacterControl(animator);
        GameObject anim = controller.animator.gameObject;
        anim.transform.parent = controller.ledgeInteraction.GrabbedLedge.transform;
        anim.transform.localPosition = controller.ledgeInteraction.GrabbedLedge.Offset;
        controller.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    public override void UpdateAbility(MACharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {

    }

    public override void OnExit(MACharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {

    }
}
