//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[CreateAssetMenu(fileName = "New State", menuName = "Assets/Scripts/Interaction/MATeleportOnLedge")]
//public class MATeleportOnLedge : MAStateData
//{
//    public override void OnEnter(MACharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
//    {

//    }
//    public override void UpdateAbility(MACharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
//    {

//    }

//    public override void OnExit(MACharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
//    {
//        MACharacterController controller = characterState.GetCharacterControl(animator);
//        MALedge ledge = controller.ledgeInteraction.GrabbedLedge;
//        Vector3 endPosition = ledge.transform.position + ledge.EndPosition;
//        controller.transform.position = endPosition;
//        controller.animator.transform.position = endPosition;
//        controller.animator.transform.parent = controller.transform;
//    }
//}
