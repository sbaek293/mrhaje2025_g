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
    public float uiDistanceOffset = -10f;
    public bool matchCameraRotation = false;
    public float iconPlacedRadius = 32;
    public float blankBetweenNode = 0.01f;

    private GameObject propertyUIContainer = null;

    private GameObject uiPivotPoint3D = null;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandleChangeWorld(int _currentWorld)
    {
        currentWorld = _currentWorld;
        if (propertyUIContainer != null && currentWorld == 0)
        {
            ClearAllPropertyUI();
        }
    }

    public void SpawnPropertyUI()
    {
        if (propertyUIContainer == null)
        {
            float distenceEnemyAndCamera = Vector3.Distance(mainCamera.transform.position, GetComponent<Transform>().position);
            Vector3 spawnPos = mainCamera.transform.position + mainCamera.transform.forward * (distenceEnemyAndCamera + uiDistanceOffset);

            uiPivotPoint3D = new GameObject("UIPivotPoint3D");
            uiPivotPoint3D.transform.position = spawnPos;

            if (matchCameraRotation)
            {
                uiPivotPoint3D.transform.rotation = mainCamera.transform.rotation;
            }

            propertyUIContainer = Instantiate(propertyContainerPrefab, uiCanvas.transform);

            //set following target
            UIFollowTarget uIFollowTarget = propertyUIContainer.GetComponent<UIFollowTarget>();
            uIFollowTarget.target = uiPivotPoint3D.transform;

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

            //setting Function of propertyUIContainer
        }
    }


    public void ClearAllPropertyUI()
    {
        if (propertyUIContainer != null)
        {
            Destroy(propertyUIContainer.gameObject);
        }
        if (uiPivotPoint3D != null)
        {
            Destroy(uiPivotPoint3D.gameObject);
        }

        propertyUIContainer = null;
    }
}
