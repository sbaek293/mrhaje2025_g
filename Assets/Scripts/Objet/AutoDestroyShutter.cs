using UnityEngine;
using System.Collections.Generic;

public class AutoDestroyShutter : MonoBehaviour
{

    [Header("Target")]
    public List<GameObject> targetObjects = new List<GameObject>();

    [Header("Sound")]
    public AudioClip destroySound;
    public float volume = 1f;

    // Update is called once per frame
    void Update()
    {
        bool allDefeated = true;

        foreach (GameObject targetObject in targetObjects) {
            if (targetObject != null && targetObject.GetComponent<EnemyFollowAI>() && !targetObject.GetComponent<EnemyFollowAI>().is_friend) {
                allDefeated = false;
            }
        }

        if (allDefeated) PlaySoundAndDestroySelf();
    }

    public void PlaySoundAndDestroySelf()
    {
        GameObject soundObj = new GameObject("TempDestroySound");
        AudioSource audioSource = soundObj.AddComponent<AudioSource>();

        audioSource.clip = destroySound;
        audioSource.volume = volume;
        audioSource.Play();

        Destroy(soundObj, destroySound.length);

        Destroy(gameObject);
    }
}
