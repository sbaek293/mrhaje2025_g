using UnityEngine;

public enum UseLimitType
{
    Time,
    Number
}

[CreateAssetMenu(fileName = "NewPropertyData", menuName = "Property System/PropertyData")]
public class PropertyDatas : ScriptableObject
{
    public string propertyName;
    [TextArea]
    public string description;
    public Sprite icon;

    public UseLimitType useLimitType;

    public AbilityScript abilityScript;

    [Header("Time")]
    public float maxDuration;

    [Header("Number")]
    public int maxNum;
}
