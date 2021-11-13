using UnityEngine;

public class MAInteractable : MonoBehaviour {
    public enum ObjectType { item, obstacle, climb, lever, image, waterwheel, elevator };
    public enum InteractionType { collider, raycast};
    public ObjectType currentSelection = ObjectType.item;
    public InteractionType currentInteraction = InteractionType.collider;

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
        if (currentSelection == ObjectType.item || currentSelection == ObjectType.climb) {
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
            case ObjectType.image:
                newHoverText = "take picture [E]";
                break;
            case ObjectType.item:
                newHoverText = "pick up [E]";
                break;
            case ObjectType.obstacle:
                newHoverText = "open [E]";
                break;
            case ObjectType.lever:
                newHoverText = "flip [E]";
                break;
            case ObjectType.climb:
                newHoverText = "climb ladder [space]";
                break;
            case ObjectType.waterwheel:
                newHoverText = "dump water [E]";
                break;
            case ObjectType.elevator:
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
