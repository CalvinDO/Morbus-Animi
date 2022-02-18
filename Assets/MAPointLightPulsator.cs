using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAPointLightPulsator : MonoBehaviour
{
    public Light light;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.light.intensity = 0.65f + .2f * Mathf.Sin(Time.timeSinceLevelLoad * 2f);
    }
}
