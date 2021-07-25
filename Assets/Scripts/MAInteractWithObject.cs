using UnityEngine;

public class MAInteractWithObject : MAInteractable
{

    public MAItem item;
    public MAInventory inventory;

    public override void MAInteract()
    {
        inventory = MAInventory.instance;
        base.MAInteract();

        if (this.currentSelection == interactType.item)
        {
            PickUp();
        }
        else
        {
            Open();
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