using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class MACollectableItem : MonoBehaviour
{


    public MAItem item;

    private bool isCharacterInReach = false;



    // Update is called once per frame
    void Update()
    {
        if (!this.isCharacterInReach) {
            return;
        }

        if (Input.GetKeyUp(KeyCode.E)) {
            MAInventory.instance.Add(item);

            GameObject.Destroy(this.gameObject);
        }


    }


    void OnTriggerStay(Collider characterCollider) {
        this.isCharacterInReach = true;

    }

    void OnTriggerExit(Collider characterCollider) {
        this.isCharacterInReach = false;

    }
}
