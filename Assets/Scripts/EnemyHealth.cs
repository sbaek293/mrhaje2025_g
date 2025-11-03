using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    int currentHealth;
    public int maxHealth;
    public Material[] materials;
    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material = materials[1];
        renderer.enabled = false;
        renderer.enabled = true;
        Invoke("changeMaterial", 1f);
        currentHealth -= amount;
        if (currentHealth <= 0)
        { Death(); }
    }
    void changeMaterial()
    {
        GetComponent<MeshRenderer>().material = materials[0];
    }
    void Death()
    {
        // Death function
        // TEMPORARY: Destroy Object
        Destroy(gameObject);
    }
}
