using Unity.Properties;
using UnityEngine;

public class Truck : MonoBehaviour
{
    public PropertyManager propertyManager;
    public Rigidbody rig;
    public Transform groundDetector;
    public LayerMask ground;
    public bool touchingWall = false;

    [Header("Forward Movement")]
    public float maxSpeed = 20f; 
    public float acceleration = 50f;

    private bool paused = false;

    public Vector3 savedVelocity;
    public Vector3 savedAngularVelocity;
    public bool savedUseGravity;
    public bool savedKinematic;

    void OnEnable() => ChangeWorld.OnChangeWorld += HandleChangeWorld;
    void OnDisable() => ChangeWorld.OnChangeWorld -= HandleChangeWorld;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (paused) return;

        bool isGrounded = true;//Physics.Raycast(groundDetector.position, Vector3.down, 0.2f, ground);

        if (isGrounded || !touchingWall)
        {
            if (propertyManager.HasPropertyName("Dash"))
            {
                Vector3 vel = rig.linearVelocity;
                float forwardSpeed = Vector3.Dot(vel, transform.forward);

                if (forwardSpeed < maxSpeed)
                {
                    Vector3 forwardDir = transform.forward;
                    forwardDir.y = 0f;
                    forwardDir.Normalize();

                    rig.AddForce(-forwardDir * rig.mass*acceleration, ForceMode.Force);
                }
            }
        } else
        {
            Debug.LogWarning("It is not on Ground!");
        }
    }

    void HandleChangeWorld(int _currentWorld)
    {
        if (_currentWorld == 1) // save kinetic datas
        {
            savedVelocity = rig.linearVelocity;
            savedAngularVelocity = rig.angularVelocity;
            savedUseGravity = rig.useGravity;
            savedKinematic = rig.isKinematic;

            rig.isKinematic = true;
            rig.useGravity = false;
            rig.linearVelocity = Vector3.zero;
            rig.angularVelocity = Vector3.zero;

            paused = true;
        }
        else // return to kinetic state
        {
            rig.isKinematic = savedKinematic;
            rig.useGravity = savedUseGravity;
            rig.linearVelocity = savedVelocity;
            rig.angularVelocity = savedAngularVelocity;

            paused = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            touchingWall = true;
        }
        else if (collision.gameObject.layer == 7)
        {
            if (collision.gameObject.GetComponent<Player>() != null)
            {
                Debug.LogWarning($"Collide with Player");
                if (!propertyManager.HasPropertyName("Heavy"))
                {
                    Vector3 forcedir = (transform.position - collision.gameObject.transform.position).normalized;
                    forcedir += Vector3.up;
                    Debug.LogWarning($"Truck : forcedir : {forcedir}");
                    rig.AddForce(forcedir * 500, ForceMode.Force);
                    //AddForceForTime(rig, 100000 * forcedir, 0.25f);
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            touchingWall = false;
        }
    }
}
