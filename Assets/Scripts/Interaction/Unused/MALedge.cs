using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MALedge : MonoBehaviour
{
    public Vector3 Offset;
    public Vector3 EndPosition;
    public static bool IsLedge(GameObject obj)
    {
        if (obj.GetComponent<MALedge>() == null)
        {
            return false;
        }

        return true;
    }
}
