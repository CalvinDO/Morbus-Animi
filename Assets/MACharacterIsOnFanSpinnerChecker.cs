using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MACharacterIsOnFanSpinnerChecker : MonoBehaviour
{
    private MAFanSpinner fanSpinner;


    void Start()
    {
        this.fanSpinner = this.transform.GetComponentInParent<MAFanSpinner>();
    }

    void OnTriggerStay(Collider characterCollider) {
        this.fanSpinner.isCharacterOnFan = true;
    }

    void OnTriggerExit(Collider characterCollider) {
        this.fanSpinner.isCharacterOnFan = false;
    }
}
