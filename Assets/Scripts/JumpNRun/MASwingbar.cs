using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MASwingbar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCharacterInitialPosition(MACharacterController characterController) {

        characterController.rb.isKinematic = true;


        GameObject characterRotator = new GameObject();
        characterRotator.name = "CharacterRotator";

        characterRotator.transform.SetPositionAndRotation(characterController.swingGrabPosition.position, characterController.swingGrabPosition.rotation);


        characterController.transform.SetParent(characterRotator.transform);
        characterRotator.transform.LookAt(this.transform);

        Vector3 handToSwingbar = this.transform.position - characterRotator.transform.position;
        characterRotator.transform.Translate(handToSwingbar);
    }
}
