using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName ="Item")]
public class MAItem : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    [Multiline]
    [Tooltip("write multiple lines of text.")]
    public string description = "Item Description";
}
