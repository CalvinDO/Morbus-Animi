using UnityEngine;

public class MAInteractable : MonoBehaviour {
    public enum objectType { item, obstacle, climb, lever, image, waterwheel, elevator };
    public enum interactionType { collider, raycast};
    public objectType currentSelection = objectType.item;
    public interactionType currentInteraction = interactionType.collider;

    public float radius = 3f;

    public GameObject textDisplay;

    public UnityEngine.UI.Text hoverTextObject;

    private Material standardMaterial;
    //private MeshRenderer meshRenderer;
    private string newHoverText;

    private void Start() {
        //this.meshRenderer = this.GetComponent<MeshRenderer>();
        //this.standardMaterial = this.meshRenderer.material;
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        if (currentSelection == objectType.item || currentSelection == objectType.climb) {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
        else {
            Gizmos.DrawWireCube(transform.position, transform.localScale * radius);
        }
    }

    public void removeHover() {


        if (this.textDisplay == null) {
            return;
        }


        textDisplay.SetActive(false);
        //this.meshRenderer.material = standardMaterial;
    }

    public void setHover() {

        if (this.textDisplay == null) {
            return;
        }

        switch (currentSelection) {
            case objectType.image:
                newHoverText = "take picture [E]";
                break;
            case objectType.item:
                newHoverText = "pick up [E]";
                break;
            case objectType.obstacle:
                newHoverText = "open [E]";
                break;
            case objectType.lever:
                newHoverText = "flip [E]";
                break;
            case objectType.climb:
                newHoverText = "climb ladder [space]";
                break;
            case objectType.waterwheel:
                newHoverText = "dump water [E]";
                break;
            case objectType.elevator:
                newHoverText = "change floor [E]";
                break;
            default:
                newHoverText = "interact [E]";
                break;
        }
        textDisplay.SetActive(true);
        hoverTextObject.text = newHoverText;
        //this.meshRenderer.sharedMaterial.SetFloat("Vector1_e2245ad420544fc4a4469d436b48ff82", 0.4f);
    }

    public virtual void MAInteract() {
        // this is overwritten
    }

    public void clearText()
    {
        hoverTextObject.text = null;
        this.textDisplay = null;
    }
}
