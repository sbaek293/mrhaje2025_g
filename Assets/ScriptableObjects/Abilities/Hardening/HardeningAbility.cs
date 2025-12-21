using UnityEngine;

[CreateAssetMenu(fileName = "HardeningAbility", menuName = "AbilityScript/HardeningAbility")]
public class HardeningAbility : AbilityScript
{
    public float org_speed;

    public override void StartAbility(Player player)
    {
        player.hardening = true;
        org_speed = player.originalSpeed;
        player.originalSpeed *= 0.2f;
    }

    public override void EndAbility(Player player)
    {
        player.hardening = false;
        player.originalSpeed = org_speed;
    }
}
