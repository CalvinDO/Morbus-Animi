using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAClimbInteraction : MAInteractable
{
    public override void MAInteract()
    {
        base.MAInteract();
        if (currentSelection == interactType.climb)
        {
            Climb();
        }
    }
    void Climb()
    {
        Destroy(this.gameObject);
        textDisplay.SetActive(false);
    }
}
