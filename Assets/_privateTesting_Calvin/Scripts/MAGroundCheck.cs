using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAGroundCheck : MonoBehaviour
{
    public bool isGrounded;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other) {
        this.isGrounded = true;
    }

    private void OnTriggerExit(Collider other) {
        this.isGrounded = false;
    }
}
