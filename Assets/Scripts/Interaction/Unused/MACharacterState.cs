//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MACharacterState : StateMachineBehaviour
//{
//    public List<MAStateData> ListAbilityData = new List<MAStateData>();
//    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//    {
//        foreach (MAStateData d in ListAbilityData)
//        {
//            d.OnEnter(this, animator, stateInfo);
//        }
//    }
//    public void UpdateAll(MACharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
//    {
//        foreach (MAStateData d in ListAbilityData)
//        {
//            d.UpdateAbility(characterState, animator, stateInfo);
//        }
//    }

//    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//    {
//        UpdateAll(this, animator, stateInfo);
//    }
//    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//    {
//        foreach (MAStateData d in ListAbilityData)
//        {
//            d.OnExit(this, animator, stateInfo);
//        }
//    }

//    private MACharacterController characterController;
//    public MACharacterController GetCharacterControl(Animator animator)
//    {
//        if (characterController == null)
//        {
//            characterController = animator.GetComponentInParent<MACharacterController>();
//        }
//        return characterController;
//    }
//}
