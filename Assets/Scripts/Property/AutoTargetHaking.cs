using UnityEngine;
using UnityEngine.UI;

public class AutoTargetHaking : MonoBehaviour
{
    [Header("Enemys")]
    public Transform enemyContainer;

    [Header("AutoTarget UI")]
    public Transform autoTargetUIContainer;
    public GameObject autoTargetPrefab;

    [Header("Enemy Property UI")]
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
    public Transform player;

    [Header("Behavior")]
    public float detectRange = 100f;
    public bool occlusionCheck = false;
    public LayerMask occlusionMask;
    public float meanNodeSelectDistence = 20;

    private Vector2 screenCenter;
    private Transform currentAutoTarget;
    private float currentTargetDistance;
    private Transform fixedTarget = null;
    public bool isTargetUIExist = false;

    private GameObject propertyUIContainer = null;
    private GameObject uiPivotPoint3D = null;

    private int selectedNodeIndex = -1;

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
        if (mainCamera == null || canvas == null || !player.GetComponent<ChangeWorld>().inCodeWorld()) return;

        if (fixedTarget == null)
        {
            //auto targeting closest enemy from mouse curser

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
        else
        {
            //check what is selected node
            EnemyPropertyManager propertyManager = fixedTarget.GetComponent<EnemyPropertyManager>();

            if (propertyManager.properties.Count > 0)
            {
                Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                Vector2 screenPos = mainCamera.WorldToScreenPoint(uiPivotPoint3D.transform.position);

                Vector2 dir = screenPos - screenCenter;
                if (dir.magnitude < meanNodeSelectDistence)
                {
                    if (selectedNodeIndex != -1)
                    {
                        propertyUIContainer.transform.GetChild(selectedNodeIndex).GetComponent<EnemyPropertyUINodeInterect>().ResetScale();
                        selectedNodeIndex = -1;
                    }
                }
                else
                {
                    float angle = Mathf.Atan2(-dir.x, -dir.y) * Mathf.Rad2Deg;
                    if (angle < 0) angle += 360f;

                    float bin = 360f / propertyManager.properties.Count;
                    int newIndex = Mathf.FloorToInt(angle / bin);

                    if (newIndex != selectedNodeIndex)
                    {
                        if (selectedNodeIndex != -1)
                        {
                            propertyUIContainer.transform.GetChild(selectedNodeIndex).GetComponent<EnemyPropertyUINodeInterect>().ResetScale();
                        }

                        propertyUIContainer.transform.GetChild(newIndex).GetComponent<EnemyPropertyUINodeInterect>().Enlarge(1.2f);
                        selectedNodeIndex = newIndex;

                        Debug.LogWarning($"angle : {angle}, newIndex : {newIndex}, enemyContainer.childCount : {enemyContainer.childCount}");
                    }
                }
            }
        }
    }

    private void Update()
    {
        OpenUIwithShift();

        CloseUIwithShift();
    }

    private void OpenUIwithClick()
    {
        if (Input.GetMouseButtonDown(0) && player.GetComponent<ChangeWorld>().inCodeWorld()) //left click -> open properties UI
        {
            if (currentAutoTarget != null && fixedTarget == null)
            {

                fixedTarget = currentAutoTarget;

                SpawnPropertyUI();
            }
        }
    }

    private void CloseUIwithClick()
    {
        if (Input.GetMouseButtonDown(1) && player.GetComponent<ChangeWorld>().inCodeWorld() && fixedTarget != null) //right click -> close properties UI
        {
            ClearPropertyUI();
            ClearTargetUI();
            fixedTarget = null;
        }
    }

    private void OpenUIwithShift()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && player.GetComponent<ChangeWorld>().inCodeWorld()) //left click -> open properties UI
        {
            if (currentAutoTarget != null && fixedTarget == null)
            {

                fixedTarget = currentAutoTarget;

                SpawnPropertyUI();
            }
        }
    }

    private void CloseUIwithShift()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift) && player.GetComponent<ChangeWorld>().inCodeWorld() && fixedTarget != null) //right click -> close properties UI
        {
            //get property if any node is selected
            if (selectedNodeIndex != -1)
            {
                PropertyDatas stolenProperty = propertyUIContainer.transform.GetChild(selectedNodeIndex).GetComponent<EnemyPropertyUINodeInterect>().StealProperty();
                PlayerPropertyManager propManager = player.GetComponent<PlayerPropertyManager>();
                propManager.AddProperty(stolenProperty);
                player.GetComponent<ChangeWorld>().changeWorldFunc();
            }

            //remove UI
            ClearPropertyUI();
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

            propertyUIContainer = Instantiate(propertyContainerPrefab, canvas.transform);

            //set following target
            UIFollowTarget uIFollowTarget = propertyUIContainer.GetComponent<UIFollowTarget>();
            uIFollowTarget.target = uiPivotPoint3D.transform;


            EnemyPropertyManager propertyManager = fixedTarget.GetComponent<EnemyPropertyManager>();
            for (int i = 0; i < propertyManager.properties.Count; i++)
            {
                PropertyDatas prop = propertyManager.GetPropertyByIndex(i);
                GameObject propertiesInstance = Instantiate(propertyUIPrefab, propertyUIContainer.transform);

                //set text, icon, click event
                Transform iconRotator = propertiesInstance.transform.Find("IconRotator");
                Image backImg = propertiesInstance.GetComponent<Image>();
                TMPro.TMP_Text label = propertiesInstance.GetComponentInChildren<TMPro.TMP_Text>();
                Image img = propertiesInstance.GetComponentsInChildren<Image>(true)[1];
                EnemyPropertyUINodeInterect propClick = propertiesInstance.GetComponent<EnemyPropertyUINodeInterect>();

                if (backImg != null) {
                    backImg.fillAmount = 1f / propertyManager.properties.Count - blankBetweenNode;
                    //Debug.LogWarning($"fillAmount : {backImg.fillAmount}");
                    backImg.transform.Rotate(0,0,-(360f / propertyManager.properties.Count)*i - 360f* blankBetweenNode/2);
                }
                if (label != null)
                {
                    label.text = prop.propertyName;
                }
                if (iconRotator != null) {
                    iconRotator.Rotate(0,0,-(360f / propertyManager.properties.Count)/2 + 360f * blankBetweenNode / 2);
                }
                if (img != null)
                {
                    img.transform.Rotate(0, 0, (360f / propertyManager.properties.Count)*(i+0.5f));
                    img.sprite = prop.icon;
                }
                if (propClick != null)
                {
                    propClick.prop = prop;
                    propClick.owner = propertyManager;
                }
            }

            selectedNodeIndex = -1;
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
