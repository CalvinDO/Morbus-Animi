using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MALevelStartManager : MonoBehaviour
{

    public bool resetCharacterPosition = true;
    public Transform characterStartPosition;

    void Start()
    {

        if (!this.resetCharacterPosition) {
            return;
        }

        MACharacterController characterController = GameObject.Find("SmallNorah").GetComponent<MACharacterController>();
        characterController.transform.position = this.characterStartPosition.position;
        characterController.physicalBody.transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
