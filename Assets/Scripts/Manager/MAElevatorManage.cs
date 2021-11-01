using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAElevatorManage : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
