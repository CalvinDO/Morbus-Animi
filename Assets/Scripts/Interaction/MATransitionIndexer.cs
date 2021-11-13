using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransitionConditionType
{
    UP,
    DOWN,
    LEFT,
    RIGHT,
    JUMP,
    GRABBING_LEDGE
}

[CreateAssetMenu(fileName = "New State", menuName = "Assets/Scripts/Interaction/MATransitionIndexer")]
public class MATransitionIndexer : MAStateData
{

    public int Index;
    public List<TransitionConditionType> transitionConditions = new List<TransitionConditionType>();

    public override void OnEnter(MACharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        MACharacterController control = characterState.GetCharacterControl(animator);
        if (MakeTransition(control))
        {
            animator.SetInteger(TransitionParameter.TransitionIndex.ToString(), Index);
        }
    }

    public override void UpdateAbility(MACharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        MACharacterController control = characterState.GetCharacterControl(animator);
        if (MakeTransition(control))
        {
            animator.SetInteger(TransitionParameter.TransitionIndex.ToString(), Index);
        }
    }

    public override void OnExit(MACharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {

    }

    private bool MakeTransition(MACharacterController control)
    {
        foreach(TransitionConditionType c in transitionConditions)
        {
            switch(c)
            {
                //case TransitionConditionType.UP:
                //    {
                //        if (!control.MoveUp)
                //        {
                //            return false;
                //        }
                //    }
                //    break;
                //case TransitionConditionType.DOWN:
                //    {
                //        if (!control.MoveDown)
                //        {
                //            return false;
                //        }
                //    }
                //    break;
                //case TransitionConditionType.LEFT:
                //    {
                //        if (!control.MoveLeft)
                //        {
                //            return false;
                //        }
                //    }
                //    break;
                //case TransitionConditionType.RIGHT:
                //    {
                //        if (!control.MoveRight)
                //        {
                //            return false;
                //        }
                //    }
                //    break;
                //case TransitionConditionType.JUMP:
                //    {
                //        if (!control.Jump)
                //        {
                //            return false;
                //        }
                //    }
                //    break;
                case TransitionConditionType.GRABBING_LEDGE:
                    {
                        if (!control.ledgeInteraction.IsGrabbingLedge)
                        {
                            return false;
                        }
                    }
                    break;
            }
        }

        return true;
    }
}
