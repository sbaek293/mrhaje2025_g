using UnityEngine;
using UnityEngine.UI;

public class EnemyPropertyManager : PropertyManager
{
    
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    GameObject effect;
    public override void RemoveProperty(PropertyDatas propData)
    {
        if (!effect)
        {
            effect = Instantiate(Resources.Load<GameObject>("PropertyEffect/HackingEffect"), transform);
            effect.transform.localPosition = new Vector3(0, 0, 0);
        }
        effect.GetComponent<ParticleSystem>().Play();

        base.RemoveProperty(propData);

        if (propData.name == "Dash")
        {
            Rigidbody rig = gameObject.GetComponent<Rigidbody>();
            Truck truck = gameObject.GetComponent<Truck>();

            truck.savedVelocity = rig.linearVelocity;
            truck.savedAngularVelocity = rig.angularVelocity;
            truck.savedUseGravity = rig.useGravity;
            truck.savedKinematic = rig.isKinematic;

            rig.isKinematic = true;
            rig.useGravity = false;
            rig.linearVelocity = Vector3.zero;
            rig.angularVelocity = Vector3.zero;
        } else if (propData.name == "Heavy")
        {
            gameObject.GetComponent<Rigidbody>().mass = 0.2f;
        } else if (propData.name == "Shield")
        {
            Transform shield = transform.Find("shield");
            if (shield != null)
            {
                Destroy(shield.gameObject);
            }
        }
    }
}
