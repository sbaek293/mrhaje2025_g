using UnityEngine;

[CreateAssetMenu(fileName = "LavitatingAbility", menuName = "AbilityScript/LavitatingAbility")]
public class LavitatingAbility : AbilityScript
{
    public override void StartAbility(Player player)
    {
        player.lavitating = true;
    }

    public override void EndAbility(Player player)
    {
        player.lavitating = false;
    }
}
