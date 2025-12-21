using UnityEngine;

[CreateAssetMenu(fileName = "HornAbility", menuName = "AbilityScript/HornAbility")]
public class HornAbility : AbilityScript
{
    public AudioClip hornSound;

    public override void StartAbility(Player player)
    {
    }

    public override void UseAbility(Player player)
    {
        AudioManager.PlaySound(player.gameObject, hornSound, false, 10, 0.1f);
    }

    public override void EndAbility(Player player)
    {
    }
}
