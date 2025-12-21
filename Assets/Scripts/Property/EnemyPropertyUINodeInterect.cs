using UnityEngine;

public class EnemyPropertyUINodeInterect : MonoBehaviour
{
    public PropertyDatas prop;
    public PropertyManager owner;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public PropertyDatas StealProperty()
    {
        if (owner != null) {
            owner.RemoveProperty(prop);
        }

        return prop;
    }

    public void Enlarge(float scaleFactor = 1.2f)
    {
        transform.localScale = originalScale * scaleFactor;
    }

    public void ResetScale()
    {
        transform.localScale = originalScale;
    }

}
