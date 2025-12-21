using UnityEngine;

[CreateAssetMenu(fileName = "MarionetteAbility", menuName = "AbilityScript/MarionetteAbility")]
public class MarionetteAbility : AbilityScript
{

    public override void StartAbility(Player player)
    {
        player.GetComponent<Weapon>().Equip(2);
    }


    public override void EndAbility(Player player)
    {
        player.GetComponent<Weapon>().Equip(0);
    }
}
