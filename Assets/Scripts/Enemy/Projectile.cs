using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    public float speed;
    public int damage;

    public LayerMask destroyLayer;


    // Start is called before the first frame update
    void Start()
    {

    }
    //
    // Update is called once per frame
    void Update()
    {
        if (PauseScript.paused || ChangeWorld.isInMatrix) { return; }
            
        // transform.Translate(Vector3.forward*Time.deltaTime*speed);
    }
            
    private void OnCollisionEnter(Collision other)
    {
        Debug.LogWarning($"Layer : {other.gameObject.layer}");
        if (((1 << other.gameObject.layer) & destroyLayer) != 0)
        {
            Debug.LogWarning("Projectile이 Destroyable에 부딫혀 삭제됨");
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().TakeDamage(damage, false, 0);
            Destroy(gameObject);
        }
    }
}
