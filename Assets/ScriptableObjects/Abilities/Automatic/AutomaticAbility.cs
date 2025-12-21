using UnityEngine;

[CreateAssetMenu(fileName = "AutomaticAbility", menuName = "AbilityScript/AutomaticAbility")]
public class AutomaticAbility : AbilityScript
{
    public override void StartAbility(Player player)
    {
        player.GetComponent<Weapon>().Equip(1);
    }

    public override void EndAbility(Player player)
    {
        player.GetComponent<Weapon>().Equip(0);
    }
}
