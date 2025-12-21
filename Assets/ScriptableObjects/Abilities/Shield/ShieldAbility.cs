using UnityEngine;

[CreateAssetMenu(fileName = "ShieldAbility", menuName = "AbilityScript/ShieldAbility")]
public class ShieldAbility : AbilityScript
{
    public GameObject shieldPrefab;

    public override void StartAbility(Player player)
    {
        GameObject shield = Instantiate(shieldPrefab, player.transform);
    }

    public override void EndAbility(Player player)
    {
        Transform shield = player.transform.Find("shieldPlayer");
        if (shield != null)
        {
            Destroy(shield.gameObject);
        }
    }
}
