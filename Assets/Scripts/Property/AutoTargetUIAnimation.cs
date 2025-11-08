using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class AutoTargetUIAnimation : MonoBehaviour
{
    [Header("Scale")]
    public Vector3 baseScale = Vector3.one;   // 최종(기본) 크기
    [Range(1f, 2f)] public float peakScale = 1.08f; // 가장 크게 커질 배수
    [Min(0f)] public float upTime = 0.08f;    // 커지는 시간
    [Min(0f)] public float downTime = 0.12f;  // 다시 줄어드는 시간
    public bool playOnEnable = true;
    public bool useUnscaledTime = true;       // UI라면 true 권장 (타임스케일 영향 X)

    [Header("Easing (비선형)")]
    // 커질 때: 천천히 시작해 빠르게 끝나는 곡선
    public AnimationCurve upCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    // 줄어들 때: 빠르게 시작해 천천히 끝나는 곡선
    public AnimationCurve downCurve = new AnimationCurve(
        new Keyframe(0f, 0f, 0f, 2.5f),
        new Keyframe(1f, 1f, 0f, 0f)
    );

    RectTransform rt;
    Coroutine routine;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        rt.localScale = baseScale;
    }

    void OnEnable()
    {
        if (playOnEnable) Play();
    }

    public void Play()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(CoPulse());
    }

    System.Collections.IEnumerator CoPulse()
    {
        Vector3 peak = baseScale * peakScale;

        // 커지기
        float t = 0f;
        while (t < upTime)
        {
            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            t += dt;
            float p = upTime > 0f ? Mathf.Clamp01(t / upTime) : 1f;
            float eased = upCurve.Evaluate(p);
            rt.localScale = Vector3.LerpUnclamped(baseScale, peak, eased);
            yield return null;
        }

        // 줄어들기
        t = 0f;
        Vector3 from = rt.localScale;
        while (t < downTime)
        {
            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            t += dt;
            float p = downTime > 0f ? Mathf.Clamp01(t / downTime) : 1f;
            float eased = downCurve.Evaluate(p);
            rt.localScale = Vector3.LerpUnclamped(from, baseScale, eased);
            yield return null;
        }

        rt.localScale = baseScale;
        routine = null;
    }
}
