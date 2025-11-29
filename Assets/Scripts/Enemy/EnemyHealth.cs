using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    int currentHealth;
    public int maxHealth;
    public Material[] materials;
    public GameObject damageTextPrefab;
    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {

        //GameObject DamageText = Instantiate(damageTextPrefab, transform);
        //DamageText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(amount.ToString());

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material = materials[1];
        renderer.enabled = false;
        renderer.enabled = true;
        Invoke("changeMaterial", 1f);

        if(GetComponent<EnemyPropertyManager>().HasPropertyName("Hardening")) currentHealth -= (int)(amount*0.2f);
        else currentHealth -= amount;

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
