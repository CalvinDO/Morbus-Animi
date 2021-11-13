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
            case ObjectType.item:
                PickUp();
                break;
            case ObjectType.obstacle:
                Open();
                break;
            case ObjectType.waterwheel:
                DumpWater();
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
    void DumpWater()
    {
        if (inventory.items.Contains(this.item))
        {
            inventory.Remove(this.item);

            Debug.Log("water wheel activated");

            textDisplay.SetActive(false);
        }
    }
}