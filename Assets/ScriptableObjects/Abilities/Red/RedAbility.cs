using UnityEngine;

[CreateAssetMenu(fileName = "RedAbility", menuName = "AbilityScript/RedAbility")]
public class RedAbility : AbilityScript
{
    public PhysicsMaterial newMaterial;
    public PhysicsMaterial oldMaterial;

    public override void StartAbility(Player player)
    {
        player.col.sharedMaterial = newMaterial;
    }

    public override void EndAbility(Player player)
    {
        player.col.sharedMaterial = oldMaterial;
    }
}
