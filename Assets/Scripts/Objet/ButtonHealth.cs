using UnityEngine;
using System.Collections.Generic;

public class ButtonHealth : EnemyHealth
{
    [Header("Target")]
    public GameObject targetObject;

    [Header("Sound")]
    public AudioClip destroySound;
    public float volume = 1f;

    [Header("InsideEnemies")]
    public List<GameObject> enemies = new List<GameObject>();
    public GameObject player;


    public override void TakeDamage(int amount, AttackType attackType = AttackType.Normal)
    {
        Debug.LogWarning("Button Damaged");
        base.TakeDamage(amount, attackType);

        TargetingPlayer();
        PlaySoundAndDestroy();
    }

    public void PlaySoundAndDestroy()
    {
        if (targetObject != null)
        {
            Destroy(targetObject);

            if (destroySound != null)
            {
                GameObject soundObj = new GameObject("TempDestroySound");
                AudioSource audioSource = soundObj.AddComponent<AudioSource>();

                audioSource.clip = destroySound;
                audioSource.volume = volume;
                audioSource.Play();

                Destroy(soundObj, destroySound.length);
            }
        }
    }

    public void TargetingPlayer()
    {
        foreach (GameObject enemy in enemies) {
            if (enemy.GetComponent<EnemyFollowAI>() != null)
            {
                enemy.GetComponent<EnemyFollowAI>().disabled = false;
                enemy.GetComponent<EnemyFollowAI>().SetTarget(player.transform);
            }
        }
    }
}
