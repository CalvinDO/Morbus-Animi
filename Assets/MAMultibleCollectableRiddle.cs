using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MAMultibleCollectableRiddle : MonoBehaviour
{
    public MAItem[] requiredItems;
    public UnityEvent triggeredEvent;

    public bool isSolved = false;


    void Update()
    {
        if (this.isSolved) {
            return;
        }

        foreach (MAItem item in this.requiredItems) {
            if (!MAInventory.instance.items.Contains(item)) {
                this.isSolved = false;
                return;
            }
        }


        this.isSolved = true;
        this.triggeredEvent.Invoke();
        
    }
}
