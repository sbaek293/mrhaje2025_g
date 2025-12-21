using UnityEngine;

[CreateAssetMenu(fileName = "FireAbility", menuName = "AbilityScript/FireAbility")]
public class FireAbility : AbilityScript
{
    public override void StartAbility(Player player)
    {
        player.GetComponent<Weapon>().Equip(5);
    }

    public override void EndAbility(Player player)
    {
        player.GetComponent<Weapon>().Equip(0);
    }
}
