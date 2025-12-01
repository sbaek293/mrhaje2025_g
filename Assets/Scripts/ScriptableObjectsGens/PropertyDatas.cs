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

    [Header("Time")]
    public float maxDuration;
    public float leftDuration;

    [Header("Number")]
    public int maxNum;
    public int leftNum;
}
