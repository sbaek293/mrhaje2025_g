using UnityEngine;

[CreateAssetMenu(fileName = "CopyAbility", menuName = "AbilityScript/CopyAbility")]
public class CopyAbility : AbilityScript
{
    public override void StartAbility(Player player)
    {
        player.GetComponent<Weapon>().Equip(3);
    }

    public override void EndAbility(Player player)
    {
        player.GetComponent<Weapon>().Equip(0);
    }
}
