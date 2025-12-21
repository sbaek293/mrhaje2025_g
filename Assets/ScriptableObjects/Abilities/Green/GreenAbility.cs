using UnityEngine;

[CreateAssetMenu(fileName = "GreenAbility", menuName = "AbilityScript/GreenAbility")]
public class GreenAbility : AbilityScript
{
    public override void StartAbility(Player player)
    {
        Debug.Log("YIPPEE GREEN START");
    }

    public override void UseAbility(Player player)
    {
        Debug.Log("YIPPEE GREEN START");
    }

    public override void EndAbility(Player player)
    {
        Debug.Log("YIPPEE GREEN END");
    }
}
