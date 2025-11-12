using UnityEngine;

public class Rabbit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private float jumpCD;
    public Rigidbody rb;
    public SphereCollider sphereCollider;
    public float jumpForce = 200f;
    public PropertyManager propertyManager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpCD <= 0)
        {
            rb.AddForce(Vector3.up * jumpForce);
            jumpCD = 5f;
        }

        if(propertyManager.properties.Count == 0)
        {
            jumpForce = 0f;

            if (sphereCollider != null)
            {
                sphereCollider.sharedMaterial = null;
            }
        }
    }

    private void FixedUpdate()
    {
         if (jumpCD > 0) jumpCD -= Time.fixedDeltaTime;
    }
}
