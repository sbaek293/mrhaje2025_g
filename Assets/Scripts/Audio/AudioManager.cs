using UnityEngine;

public static class AudioManager
{
    public static void PlaySound(GameObject gm, AudioClip audioClip,bool _2D = true,float maxdis = 20, float volume = 1,bool loop = false)
    {
        if (audioClip == null)
        {
            Debug.LogWarning("AudioClip is null. Cannot play sound.");
            return;
        }
        AudioSource audioSource;
        if (!gm.GetComponent<AudioSource>())
        {
            audioSource = gm.AddComponent<AudioSource>();
        }
        audioSource = gm.GetComponent<AudioSource>();
        audioSource.loop = loop;
        audioSource.volume = volume;
        audioSource.clip = audioClip;
        if (_2D)
        {
            audioSource.spatialBlend = 0f;
        }
        else
        {
            audioSource.spatialBlend = 1.0f;
        }
        audioSource.volume = 0.5f;
        audioSource.maxDistance = maxdis;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.Play();
    }
    public static void StopSound(GameObject gm)
    {
        gm.GetComponent<AudioSource>().Stop();
    }
}