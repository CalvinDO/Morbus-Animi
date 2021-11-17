using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MAFrustumDetector : MonoBehaviour {

    public Light light;

    [HideInInspector]
    public MACharacterController characterController;
    public LayerMask characterLayerMask;


    public bool characterDetected = false;
    [HideInInspector]
    public MACharacterController detectedCharacter;
    [HideInInspector]
    public Vector3 lastSeenCharacterPosition;

    private bool reachedOldPos = false;
    private float timeTillCalmDown = 10;
    private float remainingTimeTillCalmDown = 10;

    public MAEntityMover entityMover;

    [Range(0, 5)]
    public float catchDistance;

    public AudioSource audioSource;
    public AudioClip iSeeYou;
    public AudioClip whereAreYou;

    void Start() {
        this.characterController = GameObject.Find("SmallNorah").GetComponent<MACharacterController>();
    }


    void Update() {


        this.light.color = Color.white;

        RaycastHit hit;

        Vector3 characterConnection = this.characterController.transform.position + new Vector3(0, 1) - this.transform.position;

        Ray ray = new Ray(this.transform.position, characterConnection);

        Physics.Raycast(ray, out hit, this.light.range);


        Vector3 hitConnection = hit.point - this.transform.position;


        float angle = Vector3.Angle(this.transform.TransformDirection(Vector3.forward), hitConnection);


        if (!this.characterDetected) {
            if (angle > this.light.spotAngle) {
                return;
            }
        }


        if (this.characterWhichGotHit(hit) != null) {
            Debug.DrawRay(ray.origin, hit.point - this.transform.position);
            this.light.color = Color.yellow;

            if (!this.characterDetected) {
                this.audioSource.PlayOneShot(this.iSeeYou);
            }

            this.characterDetected = true;
            this.detectedCharacter = this.characterWhichGotHit(hit);
            this.lastSeenCharacterPosition = this.detectedCharacter.transform.position;


            float distance = Vector3.Distance(this.transform.position, hit.point);
            if (distance < this.catchDistance) {
                this.light.color = Color.red;
                Debug.Log("Game over! You got catched!");
            }

            return;
        }




        if (Vector3.Distance(this.lastSeenCharacterPosition, this.transform.position) < 2 || this.entityMover.navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid) {

            if (!this.reachedOldPos) {
                this.audioSource.PlayOneShot(this.whereAreYou);
            }

            this.reachedOldPos = true;

            this.remainingTimeTillCalmDown -= Time.deltaTime;

            Debug.Log(this.remainingTimeTillCalmDown);

            if (this.remainingTimeTillCalmDown <= 0) {
                this.characterDetected = false;
                this.reachedOldPos = false;
                this.remainingTimeTillCalmDown = this.timeTillCalmDown;
            }
        }
    }

    private MACharacterController characterWhichGotHit(RaycastHit hit) {
        if (hit.transform == null) {
            return null;
        }
        return hit.transform.gameObject.GetComponent<MACharacterController>();
    }
}
