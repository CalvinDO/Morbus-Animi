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
        MACharacterController control = characterState.GetCharacterControl(animator);
        if (MakeTransition(control))
        {
            animator.SetInteger(TransitionParameter.TransitionIndex.ToString(), 0);
        }
    }

    private bool MakeTransition(MACharacterController control)
    {
        foreach(TransitionConditionType c in transitionConditions)
        {
            switch(c)
            {
                case TransitionConditionType.UP:
                    {
                        if (!control.up)
                        {
                            return false;
                        }
                    }
                    break;
                case TransitionConditionType.DOWN:
                    {
                        if (!control.down)
                        {
                            return false;
                        }
                    }
                    break;
                case TransitionConditionType.LEFT:
                    {
                        if (!control.left)
                        {
                            return false;
                        }
                    }
                    break;
                case TransitionConditionType.RIGHT:
                    {
                        if (!control.right)
                        {
                            return false;
                        }
                    }
                    break;
                case TransitionConditionType.JUMP:
                    {
                        if (!control.jumped)
                        {
                            return false;
                        }
                    }
                    break;
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
