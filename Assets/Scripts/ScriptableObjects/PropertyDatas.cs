using UnityEngine;

[CreateAssetMenu(fileName = "NewPropertyData", menuName = "Property System/PropertyData")]
public class PropertyDatas : ScriptableObject
{
    public string propertyName;
    [TextArea]
    public string description;
    public float maxDuration;
    public float duration;
    public Sprite icon;
}
