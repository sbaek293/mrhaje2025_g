using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer, ignoredLayers;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    public float attackForceForward;
    public float attackForceUp;
    public bool is_friend = false;

    // States
    public float sightRange, attackRange;
    public float fieldOfView = 90f; // FOV in degrees
    public bool playerInSightRange, playerInAttackRange;

    public bool disabled = false;

    private Transform target;

    // For Stop at Code World
    private bool paused = false;

    void OnEnable() => ChangeWorld.OnChangeWorld += HandleChangeWorld;
    void OnDisable() => ChangeWorld.OnChangeWorld -= HandleChangeWorld;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (paused || disabled)
            return;

        // Check ranges
        playerInSightRange = IsPlayerInFOV(sightRange);
        playerInAttackRange = IsPlayerInFOV(attackRange);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private bool IsPlayerInFOV(float range)
    {
        Transform enemyContainer = transform.parent;

        if (enemyContainer != null)
        {
            foreach (Transform enemy in enemyContainer)
            {
                if (enemy == transform || enemy.GetComponent<EnemyFollowAI>() == null)
                {
                    continue; //skip self
                }

                if (is_friend ^ enemy.GetComponent<EnemyFollowAI>().is_friend)
                {
                    if (isTargetInFOV(range, enemy.transform))
                    {
                        target = enemy.transform;
                        return true;
                    }
                }
            }
        }

        if (!is_friend)
        {
            if (isTargetInFOV(range, player))
            {
                target = player;
                return true;
            }
        }

        target = null;
        return false;
    }

    private bool isTargetInFOV(float range, Transform fovtarget)
    {
        Vector3 directionToTarget = (fovtarget.position - transform.position).normalized;
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        // Check if player is within FOV angle and range
        if (angleToTarget < fieldOfView / 2f)
        {
            int mask = ~ignoredLayers;

            // Then check if nothing is blocking view
            if (Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit, range, mask))
            {
                if (hit.transform == fovtarget)
                    return true;
            }
        }
        return false;
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        if (target == null) return;
        agent.SetDestination(target.position);
    }

    private void AttackPlayer()
    {
        if (target == null) return;

        agent.SetDestination(transform.position);
        transform.LookAt(target);

        if (!alreadyAttacked)
        {
            GameObject temp_proj = Instantiate(projectile, transform.position, Quaternion.identity);
            temp_proj.GetComponent<Projectile>().is_friendly = is_friend;
            Rigidbody rb = temp_proj.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * attackForceForward, ForceMode.Impulse);
            rb.AddForce(transform.up * attackForceUp, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw range spheres
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Draw FOV vision cone for sight range
        Gizmos.color = new Color(0f, 0f, 1f, 0.4f); // semi-transparent blue

        Vector3 forward = transform.forward * sightRange;
        Vector3 leftBoundary = Quaternion.Euler(0, -fieldOfView / 2f, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, fieldOfView / 2f, 0) * forward;

        // Draw boundary rays
        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);

        // Optionally draw arc between them
#if UNITY_EDITOR
        UnityEditor.Handles.color = new Color(0f, 0.5f, 1f, 0.2f);
        UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, leftBoundary.normalized, fieldOfView, sightRange);
#endif
    }

    void HandleChangeWorld(int _currentWorld)
    {
        Rigidbody rig = GetComponent<Rigidbody>();

        if (_currentWorld == 1) // save kinetic datas
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            paused = true;
        }
        else // return to kinetic state
        {
            agent.isStopped = false;
            paused = false;
        }
    }

    public void BeMarionette()
    {
        is_friend = true;
        disabled = false;
    }

}
