using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MACharacterControlPermissions {
    public bool allowWalk = true;
    public bool allowRun = true;
    public bool allowJump = true;
    public bool allowJumpNRun = true;

    public MACharacterControlPermissions(bool allowWalk, bool allowRun, bool allowJump, bool allowJumpNRun) {
        this.allowWalk = allowWalk;
        this.allowRun = allowRun;
        this.allowJump = allowJump;
        this.allowJumpNRun = allowJumpNRun;
    }

    public MACharacterControlPermissions() {}
}

public class MAControlRestrictionTrigger : MonoBehaviour
{
    [HideInInspector]
    public bool isCharacterInTrigger = false;

    public bool allowWalk = true;
    public bool allowRun = true;
    public bool allowJump = true;
    public bool allowJumpNRun = true;


    private MACharacterController characterController;

    void Awake() {
        this.characterController = GameObject.FindObjectOfType<MACharacterController>().GetComponent<MACharacterController>();
    }

    void OnTriggerStay(Collider characterCollider) {
        this.characterController.SetPermissions(new MACharacterControlPermissions(this.allowWalk, this.allowRun, this.allowJump, this.allowJumpNRun));
        this.isCharacterInTrigger = true;
    }


    void OnTriggerExit(Collider characterCollider) {
        this.characterController.SetPermissions(new MACharacterControlPermissions());
    }
}
