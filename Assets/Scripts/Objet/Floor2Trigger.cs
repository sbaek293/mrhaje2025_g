using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Floor2Trigger : MonoBehaviour
{
    [Header("Timer")]
    public float duration = 5f;

    [Header("Audio")]
    public AudioSource timerAudioSource;
    public AudioClip knockSound;
    public float knockStartVolume = 0.5f;
    public float knockEndVolume = 1f;
    public float knockSoundInterval = 1f;
    public AudioClip destroySound;
    public float destroyVolume = 1f;

    [Header("Target")]
    public List<GameObject> targetObjects = new List<GameObject>();

    [Header("InsideEnemies")]
    public List<GameObject> enemies = new List<GameObject>();
    public GameObject player;

    private AudioSource knockAudioSource;
    private AudioSource destroyAudioSource;

    Coroutine timerCoroutine;
    private bool used = false;


    private void OnTriggerEnter(Collider other)
    {
        if (!used && other.GetComponent<Player>() != null)
        {
            Debug.LogWarning("Floor2Trigger : Player enter on trigger area");
            used = true;

            StartTimer();
        }
    }

    public void StartTimer()
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        timerCoroutine = StartCoroutine(TimerRoutine());
    }

    IEnumerator TimerRoutine()
    {
        timerAudioSource.Play();

        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (knockSound != null)
            {
                GameObject soundObj = new GameObject("TempTriggerSound");

                float knockVolume = ((duration - elapsed) / duration) * knockStartVolume + (elapsed / duration) * knockEndVolume;

                knockAudioSource = soundObj.AddComponent<AudioSource>();

                knockAudioSource.clip = knockSound;
                knockAudioSource.volume = knockVolume;

                knockAudioSource.Play();

                Destroy(soundObj, knockSound.length);
            }

            yield return new WaitForSeconds(knockSoundInterval);
            elapsed += knockSoundInterval;
        }

        timerAudioSource.Stop();

        timerCoroutine = null;

        OnTimerFinished();
    }

    protected void OnTimerFinished()
    {
        Debug.Log("Floor2Trigger : Timer End! Destroy Door");
        PlaySoundAndDestroy();
        TargetingPlayer();
    }

    public void PlaySoundAndDestroy()
    {
        foreach (GameObject targetObject in targetObjects){
            if (targetObject != null)
            {
                Destroy(targetObject);
            }
        }

        if (destroySound != null)
        {
            GameObject soundObj = new GameObject("TempTriggerSound");
            destroyAudioSource = soundObj.AddComponent<AudioSource>();

            destroyAudioSource.clip = destroySound;
            destroyAudioSource.volume = destroyVolume;

            destroyAudioSource.Play();

            Destroy(soundObj, destroySound.length);
        }
    }

    public void TargetingPlayer()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<EnemyFollowAI>() != null)
            {
                enemy.GetComponent<EnemyFollowAI>().disabled = false;
                enemy.GetComponent<EnemyFollowAI>().SetTarget(player.transform);
            }
        }
    }
}
