using System;
using System.Collections.Generic;
using UnityEngine;

public class PropertyManager : MonoBehaviour
{
    public List<PropertyDatas> properties = new List<PropertyDatas>();

    public event Action<PropertyDatas> OnAddProperty;
    public event Action<PropertyDatas> OnRemoveProperty;

    public virtual void AddProperty(PropertyDatas propData)
    {
        if (propData == null) { Debug.LogError("AddProperty: propData is NULL"); return; }
        if (properties == null) properties = new System.Collections.Generic.List<PropertyDatas>();

        properties.Add(propData);

        OnAddProperty?.Invoke(propData);
    }

    public virtual void RemoveProperty(PropertyDatas propData)
    {
        if (propData == null) { Debug.LogError("AddProperty: propData is NULL"); return; }
        if (properties == null) { Debug.LogError("AddProperty: properties is NULL"); return; }

        Debug.LogWarning($"Removeing {propData.name}");
        properties.Remove(propData);

        OnRemoveProperty?.Invoke(propData);
    }

    public virtual PropertyDatas GetPropertyByIndex(int index)
    {
        return properties[index];
    }

    public virtual bool HasPropertyName(string propertyName)
    {
        foreach (PropertyDatas propData in properties)
        {
            if (propData.propertyName.Equals(propertyName)) return true;
        }

        return false;
    }
}
