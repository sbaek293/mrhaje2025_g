using UnityEngine;

public class AutoTargetHaking : MonoBehaviour
{
    [Header("Enemys")]
    public Transform enemyContainer;

    [Header("AutoTarget UI")]
    public Transform autoTargetUIContainer;
    public GameObject autoTargetPrefab;

    [Header("Property UI")]
    public GameObject propertyUIPrefab;
    public GameObject propertyContainerPrefab;
    public Vector3 screenOffset = new Vector2(0, 0f);
    public float uiDistanceOffset = -10f;
    public bool matchCameraRotation = false;
    public float iconPlacedRadius = 32;
    public float blankBetweenNode = 0.01f;

    [Header("Refs")]
    public Camera mainCamera;
    public Canvas canvas;
    public ChangeWorld changeWorld;

    [Header("Behavior")]
    public float detectRange = 100f;
    public bool occlusionCheck = false;
    public LayerMask occlusionMask;

    private Vector2 screenCenter;
    private Transform currentAutoTarget;
    private float currentTargetDistance;
    private Transform fixedTarget = null;
    public bool isTargetUIExist = false;

    private GameObject propertyUIContainer = null;
    private GameObject uiPivotPoint3D = null;

    void OnEnable() => ChangeWorld.OnChangeWorld += HandleChangeWorld;
    void OnDisable() => ChangeWorld.OnChangeWorld -= HandleChangeWorld;

    void Awake()
    {
        if (!mainCamera) mainCamera = Camera.main;
        if (!canvas) canvas = GetComponentInParent<Canvas>();

        screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
    }

    void LateUpdate()
    {
        if (mainCamera == null || canvas == null || !changeWorld.inCodeWorld() || fixedTarget != null) return;

        Transform closestEnemy = null;
        float closestDistence = detectRange + 1;
        float distenceFromCenter = detectRange + 1;

        foreach (Transform enemy in enemyContainer)
        {
            if (enemy == null) continue;
            
            Vector3 enemyPos = enemy.position;

            // hide if ui behind camera
            Vector3 camToTarget = enemyPos - mainCamera.transform.position;
            bool behind = Vector3.Dot(mainCamera.transform.forward, camToTarget) <= 0f;
            if (behind) continue;

            // hide if ui behind some obstacles
            if (occlusionCheck)
            {
                if (Physics.Linecast(mainCamera.transform.position, enemyPos, out RaycastHit hit, occlusionMask))
                {
                    if (hit.transform != enemy && !hit.transform.IsChildOf(enemy)) continue;
                }
            }

            Vector3 screenPos = mainCamera.WorldToScreenPoint(enemyPos);
            distenceFromCenter = Vector2.Distance(screenPos, screenCenter);

            if (distenceFromCenter <= detectRange)
            {
                if (closestEnemy == null)
                {
                    closestEnemy = enemy;
                    closestDistence = distenceFromCenter;
                }
                else
                {
                    if (distenceFromCenter < closestDistence)
                    {
                        closestEnemy = enemy;
                        closestDistence = distenceFromCenter;
                    }
                }
            }
        }

        if (closestEnemy == currentAutoTarget) return;

        Debug.Log("closestEnemy == currentAutoTarget");

        ClearTargetUI();

        if (closestEnemy != null)
        {

            GameObject targetUIInstance = Instantiate(autoTargetPrefab, autoTargetUIContainer.transform);
            UIFollowTarget uiFollowTarget = targetUIInstance.GetComponentInChildren<UIFollowTarget>();
            uiFollowTarget.target = closestEnemy;

            currentAutoTarget = closestEnemy;
            currentTargetDistance = closestDistence;

            isTargetUIExist = true;
        }
    }

    private void Update()
    {
        OpenUIwithShift();

        CloseUIwithShift();
    }

    private void OpenUIwithClick()
    {
        if (Input.GetMouseButtonDown(0) && changeWorld.inCodeWorld()) //left click -> open properties UI
        {
            if (currentAutoTarget != null && fixedTarget == null)
            {

                fixedTarget = currentAutoTarget;

                EnemyPropertyManager enemyScript = fixedTarget.GetComponent<EnemyPropertyManager>();
                if (enemyScript != null)
                {
                    Debug.Log($"Calling SpawnProperty on: {fixedTarget.name}");
                    enemyScript.SpawnPropertyUI();
                }
                else
                {
                    Debug.LogWarning($"Hit enemy {fixedTarget.name} does not have Property Info!");
                }
            }
        }
    }

    private void CloseUIwithClick()
    {
        if (Input.GetMouseButtonDown(1) && changeWorld.inCodeWorld() && fixedTarget != null) //right click -> close properties UI
        {
            fixedTarget.GetComponent<EnemyPropertyManager>().ClearPropertyUI();
            ClearTargetUI();
            fixedTarget = null;
        }
    }

    private void OpenUIwithShift()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && changeWorld.inCodeWorld()) //left click -> open properties UI
        {
            if (currentAutoTarget != null && fixedTarget == null)
            {

                fixedTarget = currentAutoTarget;

                EnemyPropertyManager enemyScript = fixedTarget.GetComponent<EnemyPropertyManager>();
                if (enemyScript != null)
                {
                    Debug.Log($"Calling SpawnProperty on: {fixedTarget.name}");
                    enemyScript.SpawnPropertyUI();
                }
                else
                {
                    Debug.LogWarning($"Hit enemy {fixedTarget.name} does not have Property Info!");
                }
            }
        }
    }

    private void CloseUIwithShift()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift) && changeWorld.inCodeWorld() && fixedTarget != null) //right click -> close properties UI
        {
            //get property
            

            //remove UI
            fixedTarget.GetComponent<EnemyPropertyManager>().ClearPropertyUI();
            ClearTargetUI();
            fixedTarget = null;
        }
    }


    void ClearTargetUI()
    {
        if (!isTargetUIExist) return;

        foreach (Transform ui in autoTargetUIContainer)
        {
            Destroy(ui.gameObject);
        }
        currentAutoTarget = null;
        currentTargetDistance = detectRange + 1;
        isTargetUIExist = false;
    }

    void HandleChangeWorld(int _currentWorld)
    {
        if (propertyUIContainer != null && _currentWorld == 0)
        {
            ClearPropertyUI();
            ClearTargetUI();
        }
    }

    public void SpawnPropertyUI()
    {
        if (fixedTarget == null) return;

        if (propertyUIContainer == null)
        {
            float distenceEnemyAndCamera = Vector3.Distance(mainCamera.transform.position, fixedTarget.GetComponent<Transform>().position);
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
                PropertyDatas prop = fixedTarget.GetPropertyByIndex(i);
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


    public void ClearPropertyUI()
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
