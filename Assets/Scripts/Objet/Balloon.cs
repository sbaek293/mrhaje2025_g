using UnityEngine;

public class Ballon : MonoBehaviour
{
    public Rigidbody rb;
    public PropertyManager propertyManager;

    [Header("Sound")]
    public AudioClip destroySound;
    public float volume = 1f;


    private void OnEnable()
    {
        if (propertyManager != null)
        {
            propertyManager.OnAddProperty += HandleAddProperty;
            propertyManager.OnRemoveProperty += HandleRemoveProperty;
        }
    }

    private void OnDisable()
    {
        if (propertyManager != null)
        {
            propertyManager.OnAddProperty -= HandleAddProperty;
            propertyManager.OnRemoveProperty -= HandleRemoveProperty;
        }
    }


    private void HandleAddProperty(PropertyDatas propData)
    {
        if (propData.propertyName == "Lavitating")
        {
            rb.useGravity = false;
        }
    }

    private void HandleRemoveProperty(PropertyDatas propData)
    {
        if (propData.propertyName == "Lavitating")
        {
            rb.useGravity = true;
        }
        else if (propData.propertyName == "Bouncy")
        {
            if (destroySound != null)
            {
                GameObject soundObj = new GameObject("TempDestroySound");
                AudioSource audioSource = soundObj.AddComponent<AudioSource>();

                audioSource.clip = destroySound;
                audioSource.volume = volume;
                audioSource.Play();

                Destroy(soundObj, destroySound.length);
            }

            Destroy(gameObject);
        }
    }
}
