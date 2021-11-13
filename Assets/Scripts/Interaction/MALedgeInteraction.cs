using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MALedgeInteraction : MonoBehaviour
{
    public bool IsGrabbingLedge;
    MALedge ledge = null;
    private void OnTriggerEnter(Collider other)
    {
        ledge = other.gameObject.GetComponent<MALedge>();
        if (ledge != null)
        {
            IsGrabbingLedge = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ledge = other.gameObject.GetComponent<MALedge>();
        if (ledge != null)
        {
            IsGrabbingLedge = false;
        }
    }
}
