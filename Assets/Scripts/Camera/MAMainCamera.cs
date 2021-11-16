using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAMainCamera : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
