using UnityEngine;

[CreateAssetMenu(fileName = "JumpAbility", menuName = "AbilityScript/JumpAbility")]
public class JumpAbility : AbilityScript
{
    private float org_jump;

    public override void StartAbility(Player player)
    {
        org_jump = player.jumpForce;
        player.jumpForce += 700f;
    }

    public override void EndAbility(Player player)
    {
        player.jumpForce = org_jump;
    }
}
