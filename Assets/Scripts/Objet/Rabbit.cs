using UnityEngine;

public class Rabbit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private float jumpCD;
    public Rigidbody rb;
    public SphereCollider sphereCollider;
    public float jumpForce = 200f;
    public PropertyManager propertyManager;


    private void OnEnable()
    {
        if (propertyManager != null) {
            propertyManager.OnAddProperty += HandleAddProperty;
            propertyManager.OnRemoveProperty += HandleRemoveProperty;
        }
    }

    private void OnDisable()
    {
        if (propertyManager != null)
        {
            propertyManager.OnAddProperty -= HandleAddProperty;
            propertyManager.OnRemoveProperty -= HandleRemoveProperty;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpCD <= 0)
        {
            rb.AddForce(Vector3.up * jumpForce);
            jumpCD = 5f;
        }
    }

    private void FixedUpdate()
    {
         if (jumpCD > 0) jumpCD -= Time.fixedDeltaTime;
    }

    private void HandleAddProperty(PropertyDatas propData)
    {
        if (propData.propertyName == "Jump")
        {
            jumpForce = 200f;
        }
    }

    private void HandleRemoveProperty(PropertyDatas propData)
    {
        if (propData.propertyName == "Jump")
        {
            jumpForce = 0f;

            if (sphereCollider != null)
            {
                sphereCollider.sharedMaterial = null;
            }
        }
    }
}
