using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MALedgeInteraction : MonoBehaviour
{
    public bool IsGrabbingLedge;
    public MALedge GrabbedLedge;
    MALedge CheckLedge = null;
    private void OnTriggerEnter(Collider other)
    {
        CheckLedge = other.gameObject.GetComponent<MALedge>();
        if (CheckLedge != null)
        {
            IsGrabbingLedge = true;
            GrabbedLedge = CheckLedge;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CheckLedge = other.gameObject.GetComponent<MALedge>();
        if (CheckLedge != null)
        {
            IsGrabbingLedge = false;
            //GrabbedLedge = null;
        }
    }
}
