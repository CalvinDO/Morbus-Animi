using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MACharacterState : StateMachineBehaviour
{
    public List<MAStateData> ListAbilityData = new List<MAStateData>();

    public void UpdateAll(MACharacterState characterState, Animator animator, AnimatorStateInfo stateInf)
    {
        foreach (MAStateData d in ListAbilityData)
        {
            d.UpdateAbility(characterState, animator, stateInf);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UpdateAll(this, animator, stateInfo);
    }

    private MACharacterController characterController;
    public MACharacterController GetCharacterControl(Animator animator)
    {
        if (characterController == null)
        {
            characterController = animator.GetComponentInParent<MACharacterController>();
        }
        return characterController;
    }
}
