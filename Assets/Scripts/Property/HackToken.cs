using UnityEngine;

public class HackToken : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.CompareTag("Player"))
        {
            other.transform.Find("Main Camera").GetComponent<AutoTargetHaking>().addHackToken(1);
            Destroy(gameObject);
        }
    }
}
