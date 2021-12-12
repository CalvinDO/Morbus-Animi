using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAItemInteraction : MAInteractable
{
    public MAItem item;
    public MAInventory inventory;
    private Animator playerAnim;

    private void Start()
    {
        playerAnim = GameObject.Find("SmallNorah").GetComponent<Animator>();
    }
    public override void MAInteract()
    {
        inventory = MAInventory.instance;
        base.MAInteract();

        switch (this.objectType)
        {
            case ObjectType.obstacle:
                Open();
                break;
            case ObjectType.item:
                playerAnim.Play("Pickup");
                PickUp();
                break;
            case ObjectType.waterwheel:
                DumpWater();
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
            //textDisplay.SetActive(false);
        }
    }
    void PickUp()
    {
        MAInventory.instance.Add(this.item);
        //this is actually meant for inventory use
        switch (this.item.name)
        {
            case "Spray":
                MASprayCan sprayCan = GameObject.FindGameObjectWithTag("SprayCan").GetComponent<MASprayCan>();
                sprayCan.charges += this.item.value;
                break;
            default:
                break;
        }
        //this stays
        Destroy(this.gameObject);
        //textDisplay.SetActive(false);
    }

    void DumpWater()
    {
        if (inventory.items.Contains(this.item))
        {
            inventory.Remove(this.item);

            //Debug.Log("water wheel activated");

            //textDisplay.SetActive(false);
        }
    }
}
