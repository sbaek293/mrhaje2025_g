using UnityEngine;

public class UIFollowTarget : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public Vector3 screenOffset = new Vector3(0, 0, 0f);

    [Header("Refs")]
    public Camera viewCam;
    public Canvas canvas;

    [Header("Behavior")]
    public bool hideWhenBehind = true;
    public bool occlusionCheck = false;
    public LayerMask occlusionMask;

    RectTransform rect;

    void Awake()
    {
        rect = transform as RectTransform;
        if (!viewCam) viewCam = Camera.main;
        if (!canvas) canvas = GetComponentInParent<Canvas>();
    }

    void LateUpdate()
    {
        if (target == null || viewCam == null || canvas == null) return;

        Vector3 worldPos = target.position;

        // hide if ui behind camera
        Vector3 camToTarget = worldPos - viewCam.transform.position;
        bool behind = Vector3.Dot(viewCam.transform.forward, camToTarget) <= 0f;
        if (hideWhenBehind && behind)
        {
            if (rect.gameObject.activeSelf) rect.gameObject.SetActive(false);
            return;
        }

        // hide if ui behind some obstacles
        if (occlusionCheck)
        {
            if (Physics.Linecast(viewCam.transform.position, worldPos, out RaycastHit hit, occlusionMask))
            {
                if (hit.transform != target && !hit.transform.IsChildOf(target))
                {
                    if (rect.gameObject.activeSelf) rect.gameObject.SetActive(false);
                    return;
                }
            }
        }

        Vector3 screenPos = viewCam.WorldToScreenPoint(worldPos);

        // active ui if it is in screen
        bool onScreen = screenPos.z > 0f &&
                        screenPos.x >= 0 && screenPos.x <= Screen.width &&
                        screenPos.y >= 0 && screenPos.y <= Screen.height;

        if (onScreen)
        {
            rect.position = screenPos + screenOffset;
        }
    }
}
