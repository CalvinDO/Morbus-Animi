using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MADarkNorah : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isMoving = false;
    public Transform transformReal;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.isMoving) {
           this.transformReal.Translate(Vector3.forward * 0.1f);
        }
    }

    public void StartMoving() {
        this.isMoving = true;
    }
}
