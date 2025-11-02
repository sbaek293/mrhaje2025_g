using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerPropertyManager : PropertyManager
{

    //UI
    public GameObject propertyItemPrefab;
    public Transform propertiesContainer;

    public int maxPropertySlotNum = 2;

    private List<GameObject> propTimers = new List<GameObject>();

    private void Start()
    {
        RefreshUI();
    }

    void Update()
    {
        for (int i = properties.Count - 1; i >= 0; i--)
        {
            PropertyDatas prop = properties[i];
            if (prop == null) continue;

            prop.duration = propTimers[i].GetComponent<CircularTimer>().remainingTime;

            if (prop.duration <= 0f)
            {
                RemoveProperty(prop);
            }
            else
            {
                
            }
        }
    }


    public override void AddProperty(PropertyDatas propData)
    {
        if (properties.Count >= maxPropertySlotNum)
        {
            RemoveProperty(properties[0]);
        }

        base.AddProperty(propData);

        if (propertyItemPrefab == null) { Debug.LogError("AddProperty: propertyItemPrefab not assigned"); return; }
        if (propertiesContainer == null) { Debug.LogError("AddProperty: propertiesContainer not assigned"); return; }

        GameObject newTimer = Instantiate(propertyItemPrefab, propertiesContainer);

        //set name and image
        string displayName = propData.propertyName;
        TMP_Text tmp = newTimer.GetComponentInChildren<TMP_Text>(true);
        tmp.text = displayName;
        
        Sprite icon = propData.icon;
        Image img = newTimer.GetComponentsInChildren<Image>(true)[1];
        img.sprite = icon;

        propTimers.Add(newTimer);
    }

    public override void RemoveProperty(PropertyDatas propData)
    {
        int i = properties.IndexOf(propData);
        Destroy(propTimers[i]);
        propTimers.RemoveAt(i);

        base.RemoveProperty(propData);        
    }


    private void RefreshUI()
    {
        foreach (var item in propTimers)
        {
            Destroy(item);
        }
        propTimers.Clear();

        for (int i = 0; i < properties.Count; i++)
        {
            PropertyDatas prop = properties[i];
            GameObject newTimer = Instantiate(propertyItemPrefab, propertiesContainer);
            newTimer.GetComponentInChildren<Text>().text = prop.propertyName;
            propTimers.Add(newTimer);
        }
    }
}
