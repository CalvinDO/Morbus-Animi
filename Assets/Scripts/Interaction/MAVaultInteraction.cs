using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAVaultInteraction : MonoBehaviour
{
    public float duration;
    public float finishDuration;
    GameObject player;

    private void OnTriggerEnter(Collider collider)
    {
        player = GameObject.Find("SmallNorah");
    }
}
