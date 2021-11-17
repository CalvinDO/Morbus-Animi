using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MALevelStartManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MACharacterController characterController = GameObject.Find("SmallNorah").GetComponent<MACharacterController>();
        characterController.transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
