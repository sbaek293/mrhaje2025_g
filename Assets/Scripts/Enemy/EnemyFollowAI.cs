using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

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

    // States
    public float sightRange, attackRange;
    public float fieldOfView = 90f; // FOV in degrees
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (PauseScript.paused || ChangeWorld.isInMatrix)
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
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // Check if player is within FOV angle and range
        if (angleToPlayer < fieldOfView / 2f)
        {
            // Then check if nothing is blocking view
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, range))
            {
                if (hit.transform == player)
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
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
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

}
