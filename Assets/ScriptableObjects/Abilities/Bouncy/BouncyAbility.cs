using UnityEngine;

[CreateAssetMenu(fileName = "BouncyAbility", menuName = "AbilityScript/BouncyAbility")]
public class BouncyAbility : AbilityScript
{
    public PhysicsMaterial playerMaterial;
    public PhysicsMaterial bouncyMaterial;

    public override void StartAbility(Player player)
    {
        player.GetComponent<CapsuleCollider>().material = bouncyMaterial;
        player.rig.mass = player.originalWeight * 0.75f;
    }

    public override void EndAbility(Player player)
    {
        player.GetComponent<CapsuleCollider>().material = playerMaterial;
        player.rig.mass = player.originalWeight;
    }
}
