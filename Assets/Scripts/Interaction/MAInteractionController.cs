using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAInteractionController : MonoBehaviour
{
    //for item interaction
    MAInteractable hover;
    MASprayable wall;
    public Camera mainCamera;

    private void Update()
    {
        this.ManageRaycastInteraction();
    }
    private void OnTriggerStay(Collider collider)
    {
        MAInteractable interactable = collider.GetComponent<MAInteractable>();
        if (interactable != null)
        {
            this.hover = interactable;
            this.hover.setHover();
        }
        if (Input.GetKeyDown("space") && interactable != null)
        {
            this.hover.MAInteract();
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
