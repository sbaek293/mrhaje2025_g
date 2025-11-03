using UnityEngine;
using UnityEngine.Audio;

public class Weapon : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float attackDistance = 3f;
    public float attackDelay = 0f;
    public float attackSpeed = 1f;
    public int attackDamage = 1;
    public LayerMask attackLayer;
    public Animator animator;
    bool attacking = false;
    bool readyToAttack = true;

    public Camera cam;

    public void Attack()
    {
        if (!readyToAttack || attacking) return;

        animator.SetBool("Attack", true);
        readyToAttack = false;
        attacking = true;
        Invoke(nameof(cancelAnimation), 0.7f);
        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);
    }

    void ResetAttack()
    {
        
        attacking = false;
        readyToAttack = true;
    }
    void cancelAnimation()
    {
        animator.SetBool("Attack", false);
    }
    void AttackRaycast()
    {
        
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance, attackLayer))
        {
            if (hit.transform.TryGetComponent<EnemyHealth>(out EnemyHealth T))
            {   
                
                T.TakeDamage(attackDamage);
                Stamina.Instance.Recover(Stamina.StaminaEventType.AttackHit);
            }
        }
    }


}
