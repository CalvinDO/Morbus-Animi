using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MALevelStartManager : MonoBehaviour
{

    public bool resetCharacterPosition = true;

    void Start()
    {

        if (!this.resetCharacterPosition) {
            return;
        }

        MACharacterController characterController = GameObject.Find("SmallNorah").GetComponent<MACharacterController>();
        characterController.transform.position = Vector3.zero;
        characterController.physicalBody.transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
