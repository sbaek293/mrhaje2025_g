using UnityEngine;

public class DropItemEnemyHealth : EnemyHealth
{
    public GameObject dropItem;
    public int dropNum = 1;


    public override void Death()
    {
        for (int i = 0; i < dropNum; i++) { 
            Instantiate(dropItem, transform.position, transform.rotation);
        }

        base.Death();
    }
}
