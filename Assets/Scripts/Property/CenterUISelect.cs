using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class CenterUISelect : MonoBehaviour
{
    public Canvas canvas;
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;

    public Transform player;

    public string uiLayerName = "UI";

    void Awake()
    {
        if (canvas == null)
            canvas = FindObjectsByType<Canvas>(FindObjectsSortMode.None)[0];

        if (raycaster == null && canvas != null)
            raycaster = canvas.GetComponent<GraphicRaycaster>();

        if (eventSystem == null)
            eventSystem = FindObjectsByType<EventSystem>(FindObjectsSortMode.None)[0];
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //left click
        {
            Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = screenCenter
            };

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            if (results.Count > 0)
            {
                GameObject selectedUI = null;
                int uiLayer = LayerMask.NameToLayer(uiLayerName);

                foreach (var result in results)
                {
                    if (result.gameObject.layer == uiLayer)
                    {
                        selectedUI = result.gameObject;
                        break;
                    }
                }
                //Debug.Log($"Center UI hit: {selectedUI.name}", selectedUI);

                if (selectedUI != null)
                {
                    if (selectedUI.GetComponent<EnemyPropertyClick>() != null)
                    {
                        EnemyPropertyClick propClick = selectedUI.GetComponent<EnemyPropertyClick>();
                        PropertyDatas prop = propClick.SteelProperty();
                        PlayerPropertyManager propManager = player.GetComponent<PlayerPropertyManager>();
                        propManager.AddProperty(prop);
                        player.GetComponent<ChangeWorld>().changeWorldFunc();
                    }
                }
            }
            else
            {
                Debug.Log("Center UI hit: none");
            }
        }
    }
}
