using UnityEngine;

[CreateAssetMenu(fileName = "DashAbility", menuName = "AbilityScript/DashAbility")]
public class DashAbility : AbilityScript
{
    public override void StartAbility(Player player)
    {
        player.movementType = "truck";
        player.normalCam.fieldOfView = player.baseFOV * 1.3f;
    }

    public override void EndAbility(Player player)
    {
        player.movementType = "normal";
        player.normalCam.fieldOfView = player.baseFOV;
    }
}
