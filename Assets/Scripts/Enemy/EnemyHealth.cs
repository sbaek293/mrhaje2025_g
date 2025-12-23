using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    int currentHealth;
    public int maxHealth;
    public Material[] materials;
    public GameObject damageTextPrefab;

    public AttackType EffectiveAttack;[Tooltip("If Set to normal it takes all kinds of damages")]
    
    public delegate void DeathEvent();
    public event DeathEvent OnDeath;

    public delegate void FirstDamageEvent();
    public event FirstDamageEvent OnFirstDamage;
    private bool Damged = false;
    void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int amount, AttackType attackType = AttackType.Normal)
    {

        //GameObject DamageText = Instantiate(damageTextPrefab, transform);
        //DamageText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(amount.ToString());
        if (EffectiveAttack == AttackType.Normal || EffectiveAttack == attackType)
        {
            if (!Damged)
            {
                if (OnFirstDamage != null)
                {
                    OnFirstDamage();
                }
                Damged = true;
            }

            if (materials.Length > 0)
            {
                MeshRenderer renderer = GetComponent<MeshRenderer>();
                renderer.material = materials[1];
                renderer.enabled = false;
                renderer.enabled = true;
                Invoke(nameof(changeMaterial), 1f);
            }
            if (GetComponent<EnemyPropertyManager>())
            {
                if (GetComponent<EnemyPropertyManager>().HasPropertyName("Hardening"))
                {
                    currentHealth -= (int)(amount * 0.2f);
                }
                else
                {
                    currentHealth -= amount;
                }
            }
            else
            {
                currentHealth -= amount;
            }

            if (currentHealth <= 0)
            { Death(); }
        }
    }
    void changeMaterial()
    {
        GetComponent<MeshRenderer>().material = materials[0];
    }
    public virtual void Death()
    {
        
        // Death function
        // TEMPORARY: Destroy Object
        Destroy(gameObject);
        if (OnDeath != null)
        {
            OnDeath();
        }
    }
}
