using UnityEngine;
using UnityEngine.UI;

public class CircularTimer : MonoBehaviour
{
    [SerializeField] private Image radialImage;
    [SerializeField] private float duration = 5f;
    public float remainingTime;

    private void OnEnable()
    {
        StartTimer(duration);
    }

    public void StartTimer(float time)
    {
        duration = time;
        remainingTime = time;
        UpdateRadial();
        enabled = true;
    }

    private void Update()
    {
        remainingTime -= Time.deltaTime;
        if (remainingTime < 0f)
        {
            remainingTime = 0f;
            enabled = false;
            OnTimerFinished();
        }

        UpdateRadial();
    }

    private void UpdateRadial()
    {
        float normalized = remainingTime / duration;
        radialImage.fillAmount = normalized;
    }

    private void OnTimerFinished()
    {
        Debug.Log("end timer!");
    }
}
