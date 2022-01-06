//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[CreateAssetMenu(fileName = "New State", menuName = "Assets/Scripts/Interaction/MAToggleGravity")]
//public class MAToggleGravity : MAStateData
//{
//    public bool OnStart;
//    public bool OnEnd;
//    public bool On;

//    public override void OnEnter(MACharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
//    {
//        if(OnStart)
//        {
//            MACharacterController controller = characterState.GetCharacterControl(animator);
//            ToggleGrav(controller);
//        }
//    }

//    public override void UpdateAbility(MACharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
//    {

//    }

//    public override void OnExit(MACharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
//    {
//        if (OnEnd)
//        {
//            MACharacterController controller = characterState.GetCharacterControl(animator);
//            ToggleGrav(controller);
//        }
//    }

//    private void ToggleGrav(MACharacterController controller)
//    {
//        controller.GetComponent<Rigidbody>().velocity = Vector3.zero;
//        controller.GetComponent<Rigidbody>().useGravity = On;
//    }
//}
