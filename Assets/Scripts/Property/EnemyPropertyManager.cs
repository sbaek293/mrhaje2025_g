using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPropertyManager : PropertyManager
{
    [Header("Properties Settings")]
    public GameObject propertyUIPrefab;
    public GameObject propertyContainerPrefab;

    [Header("References")]
    public Canvas uiCanvas;
    public Camera mainCamera;

    [Header("UI")]
    public float iconPlacedRadius = 32;
    public float blankBetweenNode = 0.01f;

    private GameObject propertyUIContainer;

    private bool isUIExist = false;

    private int currentWorld = 0;

    void OnEnable() => ChangeWorld.OnChangeWorld += HandleChangeWorld;
    void OnDisable() => ChangeWorld.OnChangeWorld -= HandleChangeWorld;


    void Awake()
    {
        if (propertyUIPrefab == null)
        {
            Debug.LogWarning("Properties Prefab is not allocated!", this);
            return;
        }

        if (uiCanvas == null)
        {
            uiCanvas = FindObjectsByType<Canvas>(FindObjectsSortMode.None)[0];
            if (uiCanvas == null)
            {
                Debug.LogError("UI Canvas is not found", this);
                return;
            }
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        //make container for property UI
        propertyUIContainer = Instantiate(propertyContainerPrefab, uiCanvas.transform);

        // set UIFollowTarget script
        UIFollowTarget follow = propertyUIContainer.GetComponent<UIFollowTarget>();
        if (follow != null)
        {
            follow.target = this.transform;
            follow.viewCam = mainCamera;
            follow.canvas = uiCanvas;
        }
        else
        {
            Debug.LogWarning("UIFollowTarget Componant is not existed in Prefab.", propertyUIContainer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandleChangeWorld(int _currentWorld)
    {
        currentWorld = _currentWorld;
        if (isUIExist && currentWorld == 0)
        {
            ClearAllPropertyUI();
        }
    }

    public void SpawnPropertyUI()
    {
        if (!isUIExist)
        {
            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDatas prop = properties[i];
                GameObject propertiesInstance = Instantiate(propertyUIPrefab, propertyUIContainer.transform);

                //set text, icon, click event
                Transform iconRotator = propertiesInstance.transform.Find("IconRotator");
                Image backImg = propertiesInstance.GetComponent<Image>();
                TMPro.TMP_Text label = propertiesInstance.GetComponentInChildren<TMPro.TMP_Text>();
                Image img = propertiesInstance.GetComponentsInChildren<Image>(true)[1];
                EnemyPropertyClick propClick = propertiesInstance.GetComponent<EnemyPropertyClick>();

                if (backImg != null) {
                    backImg.fillAmount = 1f / properties.Count - blankBetweenNode;
                    Debug.LogWarning($"fillAmount : {backImg.fillAmount}");
                    backImg.transform.Rotate(0,0,-(360f / properties.Count)*i - 360f* blankBetweenNode/2);
                }
                if (label != null)
                {
                    label.text = prop.propertyName;
                }
                if (iconRotator != null) {
                    iconRotator.Rotate(0,0,-(360f / properties.Count)/2 + 360f * blankBetweenNode / 2);
                }
                if (img != null)
                {
                    img.transform.Rotate(0, 0, (360f / properties.Count)*(i+0.5f));
                    img.sprite = prop.icon;
                }
                if (propClick != null)
                {
                    propClick.prop = prop;
                    propClick.owner = this;
                }
            }

            isUIExist = true;

            //setting Function of propertyUIContainer
        }
    }


    public void ClearAllPropertyUI()
    {
        //clear children of parentContainer
        for (int i = propertyUIContainer.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(propertyUIContainer.transform.GetChild(i).gameObject);
        }

        isUIExist = false;
    }
}
