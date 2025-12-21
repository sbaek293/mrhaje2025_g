using UnityEngine;

[CreateAssetMenu(fileName = "BlueAbility", menuName = "AbilityScript/BlueAbility")]
public class BlueAbility : AbilityScript
{
    private float org_speed;

    public override void StartAbility(Player player)
    {
        org_speed = player.originalSpeed;
        player.originalSpeed += 100;
    }

    public override void EndAbility(Player player)
    {
        player.originalSpeed = org_speed;
    }
}
