using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPropertyManager : PropertyManager
{

    //UI
    public GameObject propertyItemPrefab;
    public Transform propertiesContainer;

    public Transform usedPropertyContainer;
    public GameObject propertyTimerPrefab;
    public GameObject propertyCounterPrefab;

    public int maxPropertySlotNum = 2;
    
    private List<GameObject> propIcons = new List<GameObject>();

    private PropertyDatas currentUsedAbility = null;
    private GameObject propTimer = null;
    private GameObject propCounter = null;
    private float leftDuration = 0f;
    private int leftNumber = 0;

    private Player playerScript;

    private void Start()
    {
        RefreshUI();

        playerScript = GetComponent<Player>();
    }

    void Update()
    {
        if (currentUsedAbility == null) return;

        if (currentUsedAbility.useLimitType == UseLimitType.Time)
        {
            leftDuration = propTimer.GetComponent<CircularTimer>().remainingTime;

            if (leftDuration <= 0f)
            {
                Destroy(propTimer);
                propTimer = null;
                EndCurrentProperty();
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

        GameObject newIcon = Instantiate(propertyItemPrefab, propertiesContainer);

        //set name and image
        string displayName = propData.propertyName;
        TMP_Text tmp = newIcon.GetComponentInChildren<TMP_Text>(true);
        tmp.text = displayName;
        
        Sprite icon = propData.icon;
        Image img = newIcon.GetComponentsInChildren<Image>(true)[1];
        img.sprite = icon;

        propIcons.Add(newIcon);
    }

    public override void RemoveProperty(PropertyDatas propData)
    {
        int i = properties.IndexOf(propData);
        Destroy(propIcons[i]);
        propIcons.RemoveAt(i);

        base.RemoveProperty(propData);        
    }

    private void RefreshUI()
    {
        foreach (var item in propIcons)
        {
            Destroy(item);
        }
        propIcons.Clear();
    }

    public void UseProperty()
    {
        if (currentUsedAbility == null) { 
            if (properties.Count > 0)
            {
                currentUsedAbility = properties[0];

                if (currentUsedAbility.useLimitType == UseLimitType.Number)
                {
                    leftNumber = currentUsedAbility.maxNum;

                    propCounter = Instantiate(propertyCounterPrefab, usedPropertyContainer);
                    //set name and image
                    string displayName = currentUsedAbility.propertyName;
                    TMP_Text tmp = propCounter.transform.Find("name").GetComponent<TMP_Text>();
                    tmp.text = displayName;

                    Sprite icon = currentUsedAbility.icon;
                    Image img = propCounter.GetComponentsInChildren<Image>(true)[1];
                    img.sprite = icon;

                    TMP_Text countTmp = propCounter.transform.Find("Count").GetComponent<TMP_Text>();
                    countTmp.text = leftNumber.ToString();
                }
                else if (currentUsedAbility.useLimitType == UseLimitType.Time)
                {
                    propTimer = Instantiate(propertyTimerPrefab, usedPropertyContainer);
                    //set name and image
                    string displayName = currentUsedAbility.propertyName;
                    TMP_Text tmp = propTimer.GetComponentInChildren<TMP_Text>(true);
                    tmp.text = displayName;

                    Sprite icon = currentUsedAbility.icon;
                    Image img = propTimer.GetComponentsInChildren<Image>(true)[1];
                    img.sprite = icon;

                    CircularTimer timer = propTimer.GetComponent<CircularTimer>();
                    timer.StartTimer(currentUsedAbility.maxDuration);
                }

                currentUsedAbility.abilityScript.StartAbility(playerScript);
                RemoveProperty(properties[0]);
            }
        }
        else
        {
            currentUsedAbility.abilityScript.UseAbility(playerScript);
        }
    }

    public void EndCurrentProperty()
    {
        if (currentUsedAbility == null) return;

        currentUsedAbility.abilityScript.EndAbility(playerScript);

        currentUsedAbility = null;
        propTimer = null;
        propCounter = null;
        leftDuration = 0f;
        leftNumber = 0;
    }


    public void countUseNumber()
    {
        if (currentUsedAbility == null) return;

        if (currentUsedAbility.useLimitType == UseLimitType.Number)
        {
            leftNumber -= 1;

            TMP_Text countTmp = propCounter.transform.Find("Count").GetComponent<TMP_Text>();
            countTmp.text = leftNumber.ToString();

            if (leftNumber <= 0)
            {
                Destroy(propCounter);
                propCounter = null;
                EndCurrentProperty();
            }
        }
    }
}
