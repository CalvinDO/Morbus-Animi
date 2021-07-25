using UnityEngine;
using UnityEngine.UI;

public class MAInteractWithObject : MAInteractable
{

    public MAItem item;
    public MAInventory inventory;

    public override void MAInteract()
    {
        inventory = MAInventory.instance;
        base.MAInteract();

        switch (currentSelection)
        {
            case interactType.item:
                PickUp();
                break;
            case interactType.obstacle:
                Open();
                break;
            default:
                break;
        }
    }

    void PickUp()
    {
        MAInventory.instance.Add(this.item);
        Destroy(this.gameObject);
        textDisplay.SetActive(false);
    }

    void Open()
    {
        if (inventory.items.Contains(this.item))
        {
            Destroy(this.gameObject);
            textDisplay.SetActive(false);
        }
    }
}