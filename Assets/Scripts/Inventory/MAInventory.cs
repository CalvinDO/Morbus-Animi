using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAInventory : MonoBehaviour
{

    #region Singleton

    public static MAInventory instance;
    void Awake ()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public List<MAItem> items = new List<MAItem>();

    public void Add (MAItem item)
    {
        items.Add(item);

        if(onItemChangedCallback != null)
           onItemChangedCallback.Invoke();
    }

    public void Remove(MAItem item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
