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
}
