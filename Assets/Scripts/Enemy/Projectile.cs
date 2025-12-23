using UnityEngine;


public class Projectile : MonoBehaviour
{
    public float speed;
    public int damage;

    public LayerMask destroyLayer;
    public bool is_friendly = false;


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
        //Debug.LogWarning($"other.collider.gameObject.layer : {other.collider.gameObject.layer}");
        if (((1 << other.collider.gameObject.layer) & destroyLayer) != 0)
        {
            Destroy(gameObject);
        }
        else if (!is_friendly && other.collider.gameObject.CompareTag("Player") && gameObject.layer == 11)
        {
            other.gameObject.GetComponent<Player>().TakeDamage(damage, false, 0);
            Destroy(gameObject);
        }
        else if (other.collider.gameObject.layer == 6) // 6 == enemy layer
        {
            if (other.gameObject.GetComponent<EnemyFollowAI>() == null)
            {
                if (gameObject.layer == 12)
                {
                    if(other.gameObject.GetComponent<EnemyHealth>() != null) other.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
                    Destroy(gameObject);
                }
            } else
            {
                if (gameObject.layer == 12)
                {
                    if (!other.gameObject.GetComponent<EnemyFollowAI>().is_friend)
                    {
                        if (other.gameObject.GetComponent<EnemyHealth>() != null) other.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
                        Destroy(gameObject);
                    }
                } 
                else if (gameObject.layer == 11)
                {
                    if (is_friendly ^ other.gameObject.GetComponent<EnemyFollowAI>().is_friend)
                    {
                        other.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
