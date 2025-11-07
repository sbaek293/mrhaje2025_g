using UnityEngine;

public class EnemyPropertyClick : MonoBehaviour
{
    public PropertyDatas prop;
    public EnemyPropertyManager owner;

    public PropertyDatas SteelProperty()
    {
        if (owner != null) {
            owner.RemoveProperty(prop);
        }

        return prop;
    }
}
