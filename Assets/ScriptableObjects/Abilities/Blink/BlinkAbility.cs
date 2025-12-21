using UnityEngine;

[CreateAssetMenu(fileName = "BlinkAbility", menuName = "AbilityScript/BlinkAbility")]
public class BlinkAbility : AbilityScript
{
    public float blinkDistance = 10;

    public override void StartAbility(Player player)
    {
    }

    public override void UseAbility(Player player)
    {
        float t_hmove = Input.GetAxisRaw("Horizontal");
        float t_vmove = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(t_hmove, 0, t_vmove).normalized;
        if (dir.magnitude == 0) dir = new Vector3(0, 0, 1);
        Vector3 diff = dir * blinkDistance;
        player.transform.position = player.transform.position + player.transform.TransformDirection(diff);

        player.GetComponent<PlayerPropertyManager>().countUseNumber();
    }

    public override void EndAbility(Player player)
    {
    }
}
