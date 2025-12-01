using UnityEngine;

public class Door : MonoBehaviour
{
    public AudioClip audioClip;
    public bool DoorLocked;
    public Rigidbody[] Hinges;
    private void Start()
    {
            for(int i=0; i < Hinges.Length; i++)
            {
                Hinges[i].isKinematic = DoorLocked;
            }
    }

    public void DoorTrigger(bool Unlock)
    {
        if (audioClip)
        {
            AudioManager.PlaySound(gameObject, audioClip, false, 10, 0.5f);
        }
        DoorLocked = Unlock;
        for (int i = 0; i < Hinges.Length; i++)
        {
            Hinges[i].isKinematic = DoorLocked;
        }
    }
}
