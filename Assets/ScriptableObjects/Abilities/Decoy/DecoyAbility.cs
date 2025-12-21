using UnityEngine;

[CreateAssetMenu(fileName = "DecoyAbility", menuName = "AbilityScript/DecoyAbility")]
public class DecoyAbility : AbilityScript
{
    public GameObject playerDecoyPrefab;

    public override void StartAbility(Player player)
    {
    }

    public override void UseAbility(Player player)
    {
        Transform enemyContainer = GameObject.Find("EnemyContainer").transform;
        GameObject temp_decop = Instantiate(playerDecoyPrefab, enemyContainer);
        temp_decop.transform.position = player.transform.position;

        Debug.LogWarning($"player position : {player.transform.position}");
        Debug.LogWarning($"temp_decop position : {temp_decop.transform.position}");

        player.GetComponent<PlayerPropertyManager>().countUseNumber();
    }

    public override void EndAbility(Player player)
    {
    }
}
