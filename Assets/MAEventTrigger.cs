using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Collider))]
public class MAEventTrigger : MonoBehaviour {

    public UnityEvent triggeredEvent;


    void OnTriggerEnter(Collider characterCollider) {

        if (this.triggeredEvent == null) {
            return;
        }
        this.triggeredEvent.Invoke();
    }
}
