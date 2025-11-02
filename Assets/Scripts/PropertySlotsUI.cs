using Unity.Properties;
using UnityEngine;

public class PropertySlotsUI : MonoBehaviour
{
    public PlayerPropertyManager playerPropManager;
    public GameObject propertySlotPrefab;

    void Awake()
    {
        for (int i = 0; i < playerPropManager.maxPropertySlotNum; i++)
        {
            GameObject propertiesInstance = Instantiate(propertySlotPrefab, this.gameObject.transform);
        }
    }
}
