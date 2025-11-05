using UnityEngine;

public class AutoTargetHaking : MonoBehaviour
{
    [Header("Enemys")]
    public Transform enemyContainer;
    public Transform autoTargetUIContainer;
    public GameObject autoTargetPrefab;
    public Vector3 screenOffset = new Vector2(0, 0f);

    [Header("Refs")]
    public Camera viewCam;
    public Canvas canvas;

    [Header("Behavior")]
    public float detectRange = 100f;
    public bool occlusionCheck = false;
    public LayerMask occlusionMask;

    private Vector2 screenCenter;

    private Transform currentAutoTarget;
    private float currentTargetDistance;

    public bool isTargetUIExist = false;

    void Awake()
    {
        if (!viewCam) viewCam = Camera.main;
        if (!canvas) canvas = GetComponentInParent<Canvas>();

        screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
    }

    void LateUpdate()
    {
        if (viewCam == null || canvas == null) return;

        Transform closestEnemy = null;
        float closestDistence = detectRange + 1;
        float distenceFromCenter = detectRange + 1;

        foreach (Transform enemy in enemyContainer)
        {
            if (enemy == null) continue;
            
            Vector3 enemyPos = enemy.position;

            // hide if ui behind camera
            Vector3 camToTarget = enemyPos - viewCam.transform.position;
            bool behind = Vector3.Dot(viewCam.transform.forward, camToTarget) <= 0f;
            if (behind) continue;

            // hide if ui behind some obstacles
            if (occlusionCheck)
            {
                if (Physics.Linecast(viewCam.transform.position, enemyPos, out RaycastHit hit, occlusionMask))
                {
                    if (hit.transform != enemy && !hit.transform.IsChildOf(enemy)) continue;
                }
            }

            Vector3 screenPos = viewCam.WorldToScreenPoint(enemyPos);
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
}
