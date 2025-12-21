using UnityEngine;

[CreateAssetMenu(fileName = "HeavyAbility", menuName = "AbilityScript/HeavyAbility")]
public class HeavyAbility : AbilityScript
{
    private float org_speed;

    public override void StartAbility(Player player)
    {
        org_speed = player.originalSpeed;
        player.rig.mass = player.originalWeight * 5;
        player.originalSpeed *= 0.5f;
    }

    public override void EndAbility(Player player)
    {
        player.rig.mass = player.originalWeight;
        player.originalSpeed = org_speed;
    }
}
