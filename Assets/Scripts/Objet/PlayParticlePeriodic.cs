using UnityEngine;
using System.Collections;

public class PlayParticle : MonoBehaviour
{
    public float baseInterval = 1.0f;
    public float randomOffset = 0.2f;

    ParticleSystem ps;
    Coroutine loop;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        if (ps == null) return;

        loop = StartCoroutine(PlayLoop());
    }

    IEnumerator PlayLoop()
    {
        while (true)
        {
            ps.Play();

            float interval = baseInterval + Random.Range(-randomOffset, randomOffset);
            interval = Mathf.Max(0.05f, interval);

            yield return new WaitForSeconds(interval);
        }
    }

    void OnDisable()
    {
        if (loop != null) StopCoroutine(loop);
    }
}
