using UnityEngine;

[CreateAssetMenu(fileName = "NewSkin", menuName = "Create New Skin")]
public class SSkinInfo : ScriptableObject
{
    public enum SkinIDs
    {
        purple,
        red,
        green,
        blue,
        cyan,
        pink,
        dark_purple,
        black,
        orange,
        light_blue
    }

    [Tooltip("Unique ID for this skin (used by the selector).")]
    public SkinIDs skinID;

    [Tooltip("Optional sprite associated with this skin (not required if you toggle GameObjects).")]
    public Sprite skinSprite;

    [Tooltip("Optional display name for editor clarity.")]
    public string displayName;
    [Tooltip("Minimum player level required to unlock this skin.")]
    public int requiredLevel = 0;
}