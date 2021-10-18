using UnityEngine;
using UnityEngine.UI;

public class MAInventorySlot : MonoBehaviour
{
    public Image icon; 

    MAItem item;

    public void AddItem (MAItem newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void RemoveItem()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
    }
}
