using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAInventoryUI : MonoBehaviour
{
    public Transform itemsParent;

    MAInventory inventory;

    MAInventorySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        inventory = MAInventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<MAInventorySlot>();
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].RemoveItem();
            }
        }
    }
}
