using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MACharacterManager : Singleton<MACharacterManager>
{
    public List<MACharacterController> Characters = new List<MACharacterController>();

    public MACharacterController GetMACharacter(Animator animator)
    {
        foreach (MACharacterController controller in Characters)
        {
            if (controller.animator == animator)
            {
                return controller;
            }
        }
        return null;
    }
}
