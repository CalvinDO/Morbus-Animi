using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAItemInteraction : MAInteractable
{
    public MAItem item;
    public MAInventory inventory;
    public override void MAInteract()
    {
        inventory = MAInventory.instance;
        base.MAInteract();

        switch (this.currentSelection)
        {
            case interactType.obstacle:
                Open();
                break;
            case interactType.item:
                PickUp();
                break;
            default:
                break;
        }
    }
    void Open()
    {
        if (inventory.items.Contains(this.item))
        {
            Destroy(this.gameObject);
            textDisplay.SetActive(false);
        }
    }
    void PickUp()
    {
        MAInventory.instance.Add(this.item);
        Destroy(this.gameObject);
        textDisplay.SetActive(false);
    }
}
