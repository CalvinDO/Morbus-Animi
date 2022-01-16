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

    public Animator animator;


    void Start() {
        this.characterController = GameObject.Find("SmallNorah").GetComponent<MACharacterController>();

        this.animator.SetBool("isWalking", true);
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


        if (this.GetCharacterWhichGotHit(hit) != null) {

            this.DetectCharacter(ray, hit);

            return;
        }


        if (this.entityMover.isStationary) {
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

                this.ReturnToRoaming();
            }
        }
    }


    private void DetectCharacter(Ray ray, RaycastHit hit) {

        Debug.DrawRay(ray.origin, hit.point - this.transform.position);
        this.light.color = Color.yellow;

        if (!this.characterDetected) {
            this.audioSource.PlayOneShot(this.iSeeYou);
        }

        this.characterDetected = true;
        this.detectedCharacter = this.GetCharacterWhichGotHit(hit);
        this.lastSeenCharacterPosition = this.detectedCharacter.transform.position;

        if (this.entityMover.isStationary) {
            this.light.color = Color.red;
            this.characterController.Die();
        }
        

        float distance = Vector3.Distance(this.transform.position, hit.point);
        if (distance < this.catchDistance) {
            this.light.color = Color.red;
            Debug.Log("Game over! You got catched!");
            this.characterController.Die();
        }

        this.animator.SetBool("isSprinting", true);
        this.animator.SetBool("isWalking", false);
    }




    private void ReturnToRoaming() {
        this.characterDetected = false;
        this.reachedOldPos = false;
        this.remainingTimeTillCalmDown = this.timeTillCalmDown;

        this.entityMover.LostCharacter();

        this.animator.SetBool("isSprinting", false);
        this.animator.SetBool("isWalking", true);
    }

    private MACharacterController GetCharacterWhichGotHit(RaycastHit hit) {
        if (hit.transform == null) {
            return null;
        }
        return hit.transform.gameObject.GetComponent<MACharacterController>();
    }
}
