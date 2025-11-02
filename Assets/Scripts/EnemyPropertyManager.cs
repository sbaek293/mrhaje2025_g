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

    private GameObject propertyUIContainer;

    private bool isUIExist = false;

    private int currentWorld = 0;

    void OnEnable() => Player.OnChangeWorld += HandleChangeWorld;
    void OnDisable() => Player.OnChangeWorld -= HandleChangeWorld;


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
            follow.worldOffset = new Vector3(2.0f, 2.0f, 0f);
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
                TMPro.TMP_Text label = propertiesInstance.GetComponentInChildren<TMPro.TMP_Text>();
                Image img = propertiesInstance.GetComponentsInChildren<Image>(true)[1];
                EnemyPropertyClick propClick = propertiesInstance.GetComponent<EnemyPropertyClick>();

                if (label != null)
                {
                    label.text = prop.propertyName;
                }
                if (img != null)
                {
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
