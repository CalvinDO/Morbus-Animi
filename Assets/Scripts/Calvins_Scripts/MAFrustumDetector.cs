using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAFrustumDetector : MonoBehaviour {

    public Light light;
    public MACharacterController characterController;
    public LayerMask characterLayerMask;


    void Start() {
    }


    void Update() {
        this.light.color = Color.white;

        RaycastHit hit;

        Vector3 characterConnection = (this.characterController.transform.position + new Vector3(0, 1)) - this.transform.position;

        Ray ray = new Ray(this.transform.position, characterConnection);

        Physics.Raycast(ray, out hit, this.light.range);

        
        Vector3 hitConnection = hit.point - this.transform.position;


        float angle = Vector3.Angle(this.transform.TransformDirection(Vector3.forward), hitConnection);


        if (angle > this.light.spotAngle) {
            return;
        }

        
        if (this.characterWhichGotHit(hit) != null) {
            Debug.DrawRay(ray.origin, hit.point - this.transform.position);
            this.light.color = Color.red;
        }
    }

    private MACharacterController characterWhichGotHit(RaycastHit hit) {
        if (hit.transform == null) {
            return null;
        }
        return hit.transform.gameObject.GetComponent<MACharacterController>();
    }
}
