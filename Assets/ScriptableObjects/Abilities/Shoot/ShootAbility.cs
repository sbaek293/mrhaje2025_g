using UnityEngine;

[CreateAssetMenu(fileName = "ShootAbility", menuName = "AbilityScript/ShootAbility")]
public class ShootAbility : AbilityScript
{

    public override void StartAbility(Player player)
    {
        player.GetComponent<Weapon>().Equip(4);
    }


    public override void EndAbility(Player player)
    {
        player.GetComponent<Weapon>().Equip(0);
    }
}
