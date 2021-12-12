using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAInteractionController : MonoBehaviour
{
    //for item interaction
    MAInteractable hover;
    MASprayable wall;
    Camera mainCamera;

    private void Awake()
    {
        mainCamera = this.GetComponent<MACharacterController>().mainCamera;
    }
    private void Update()
    {
        this.ManageRaycastInteraction();
    }
    private void OnTriggerStay(Collider collider)
    {
        //Debug.Log("collided");
        Rigidbody PlayerRB = this.GetComponent<Rigidbody>();
        MAInteractable interactable = collider.GetComponent<MAInteractable>();
        MAElevatorInteraction elevatorInteraction = collider.GetComponent<MAElevatorInteraction>();
        MAPushable pushable = collider.GetComponent<MAPushable>();
        if (interactable != null)
        {
            this.hover = interactable;
            this.hover.setHover();
        }
        if (Input.GetKeyDown("e") && interactable != null && elevatorInteraction == null)
        {
            this.hover.MAInteract();
        }
        if (Input.GetKeyDown("e") && elevatorInteraction != null)
        {
            this.hover.MAInteract();
        }
        if (Input.GetKey("e") && pushable != null)
        {
            Debug.Log("entered");
            pushable.GetComponent<Rigidbody>().velocity = new Vector3(PlayerRB.velocity.x, PlayerRB.velocity.y, PlayerRB.velocity.z);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (this.hover != null)
        {
            this.hover.removeHover();
            this.hover = null;
        }
    }

    private void ManageRaycastInteraction()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            MAInteractable interactable = hit.collider.GetComponent<MAInteractable>();
            MASprayable sprayable = hit.collider.GetComponent<MASprayable>();
            if (interactable != null && interactable.currentInteraction.Equals(MAInteractable.InteractionType.raycast))
            {
                this.hover = interactable;
                this.hover.setHover();
            }
            else
            {
                if (this.hover != null)
                {
                    this.hover.removeHover();
                    this.hover = null;
                }
            }
            if (this.hover == interactable)
            {
                if (Input.GetMouseButtonDown(0) && interactable != null)
                {
                    this.hover.MAInteract();
                }
            }
            if (sprayable != null && Input.GetMouseButtonDown(1))
            {
                this.wall = sprayable;
                Vector3 difference = this.transform.position - hit.point;
                this.wall.Spray(hit.point, sprayable.transform.rotation, difference);
            }
            else
            {
                this.wall = null;
            }
        }
    }
}
