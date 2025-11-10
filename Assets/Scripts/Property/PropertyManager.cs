using System.Collections.Generic;
using UnityEngine;

public class PropertyManager : MonoBehaviour
{
    public List<PropertyDatas> properties = new List<PropertyDatas>();

    public virtual void AddProperty(PropertyDatas propData)
    {
        if (propData == null) { Debug.LogError("AddProperty: propData is NULL"); return; }
        if (properties == null) properties = new System.Collections.Generic.List<PropertyDatas>();

        propData.duration = propData.maxDuration;

        properties.Add(propData);
    }

    public virtual void RemoveProperty(PropertyDatas propData)
    {
        if (propData == null) { Debug.LogError("AddProperty: propData is NULL"); return; }
        if (properties == null) { Debug.LogError("AddProperty: properties is NULL"); return; }

        properties.Remove(propData);
    }
    
    public virtual PropertyDatas GetPropertyByIndex(int index)
    {
        return properties[index];
    }
}
