using UnityEngine;

public class UIFollowTarget : MonoBehaviour
{
    [Header("Target")]
    public Transform target;          // 이름표를 띄울 월드 오브젝트
    public Vector3 worldOffset = new Vector3(0, 2.0f, 0); // 머리 위 오프셋(미터 단위)

    [Header("Refs")]
    public Camera viewCam;            // 카메라 (null이면 Camera.main)
    public Canvas canvas;             // 상위 캔버스 (Screen Space - Overlay)

    [Header("Behavior")]
    public bool hideWhenBehind = true;      // 카메라 뒤면 숨김
    public bool occlusionCheck = false;     // 가림(벽 등) 체크할지
    public LayerMask occlusionMask;         // 가림 판정 레이어

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

        // 월드에서 살짝 위로 올린 피벗 위치
        Vector3 worldPos = target.position + worldOffset;

        // 카메라 뒤면 숨김
        Vector3 camToTarget = worldPos - viewCam.transform.position;
        bool behind = Vector3.Dot(viewCam.transform.forward, camToTarget) <= 0f;
        if (hideWhenBehind && behind)
        {
            if (rect.gameObject.activeSelf) rect.gameObject.SetActive(false);
            return;
        }

        // (옵션) 가림 체크: 벽/지형이 타겟을 가리면 숨김
        if (occlusionCheck)
        {
            if (Physics.Linecast(viewCam.transform.position, worldPos, out RaycastHit hit, occlusionMask))
            {
                // 히트가 타겟 자체가 아니면 가려진 것으로 간주
                if (hit.transform != target && !hit.transform.IsChildOf(target))
                {
                    if (rect.gameObject.activeSelf) rect.gameObject.SetActive(false);
                    return;
                }
            }
        }

        // 스크린 좌표로 변환
        Vector3 screenPos = viewCam.WorldToScreenPoint(worldPos);

        // 화면 안이면 표시
        bool onScreen = screenPos.z > 0f &&
                        screenPos.x >= 0 && screenPos.x <= Screen.width &&
                        screenPos.y >= 0 && screenPos.y <= Screen.height;

        if (!onScreen)
        {
            Debug.Log($"[UIFollowTarget] Property is unactive, target: {target.name}, screenPos: {screenPos}, onScreen: {onScreen}", this.gameObject);
        }

        //rect.gameObject.SetActive(onScreen);

        if (onScreen)
        {
            // Screen Space - Overlay라면 그냥 position에 스크린 좌표 대입
            rect.position = screenPos;         // 픽셀 좌표
            // 크기는 RectTransform 자체의 크기로 고정 → 거리와 무관하게 일정
        }
    }
}
