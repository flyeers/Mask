using UnityEngine;

[CreateAssetMenu(fileName = "Mask", menuName = "Scriptable Objects/MaskSO")]
public class MaskSO : ScriptableObject
{
    public string Name;
    public Sprite MaskSprite;
    public string AbilityScriptName;

    public Sprite MaskSpriteRight;
    public Sprite MaskSpriteLeft;
}
