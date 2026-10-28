using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class HostileAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask playerLayerMask;

    [SerializeField] private float patrolSpeed = 2.5f;
    [SerializeField] private float chaseSpeed = 9f;
    [SerializeField] private float patrolRadius = 10f;

    [SerializeField] private float visionRange = 20f;
    [SerializeField] private float engagementRange = 10f;

    [SerializeField] private float maxHealth = 100f;

    [SerializeField] private float chaseTurnSpeed = 720f;

    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float meleeRange = 2f;
    private float slowdown = 0f; 
    private bool isDead = false;

    private float currentHealth;
    private Vector3 currentPatrolPoint;
    private bool hasPatrolPoint;
    private bool isPlayerVisible;
    private bool isPlayerInRange;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (navAgent == null)
            navAgent = GetComponent<NavMeshAgent>();

        if (navAgent != null)
        {
            navAgent.enabled = true;
            navAgent.isStopped = false;
            navAgent.updatePosition = true;
            navAgent.updateRotation = true;

            navAgent.angularSpeed = 720f;
            navAgent.acceleration = 30f;
            navAgent.autoBraking = false;
        }

        if (animator == null)
            animator = GetComponent<Animator>();

        if (playerTransform == null)
        {
            GameObject player = GameObject.Find("Player");

            if (player != null)
                playerTransform = player.transform;
        }
    }

    private void Start()
    {
        FindPatrolPoint();
    }

    private void Update()
    {
        if (isDead)
            return;

        DetectPlayer();
        UpdateBehaviourState();
    }

    private void DetectPlayer()
    {
        if (playerTransform == null)
        {
            isPlayerVisible = false;
            isPlayerInRange = false;
            return;
        }

        isPlayerVisible = Physics.CheckSphere(transform.position, visionRange, playerLayerMask);
        isPlayerInRange = Physics.CheckSphere(transform.position, engagementRange, playerLayerMask);
    }

    private void UpdateBehaviourState() 
    {
        if (!isPlayerVisible && !isPlayerInRange)
        {
            Patrol();
        }
        else if (isPlayerVisible && !isPlayerInRange)
        {
            Chase();
        }
        else
        {
            Attack();
        }
    }

    private void Patrol() 
    {
        if (navAgent == null || !navAgent.enabled)
            return;

        navAgent.isStopped = false;
        navAgent.updateRotation = true;
        navAgent.speed = patrolSpeed;

        if (!hasPatrolPoint)
        {
            FindPatrolPoint();
        }

        if (hasPatrolPoint)
        {
            navAgent.SetDestination(currentPatrolPoint);

            if (!navAgent.pathPending && navAgent.remainingDistance <= 1f)
            {
                hasPatrolPoint = false;
            }
        }
    }

    private void FindPatrolPoint()
    {
        if (navAgent == null || !navAgent.enabled)
            return;

        for (int i = 0; i < 20; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
            randomDirection.y = 0f;

            Vector3 randomPoint = transform.position + randomDirection;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, patrolRadius, NavMesh.AllAreas))
            {
                currentPatrolPoint = hit.position;
                hasPatrolPoint = true;
                return;
            }
        }

        NavMeshHit currentHit;
        if (NavMesh.SamplePosition(transform.position, out currentHit, 5f, NavMesh.AllAreas))
        {
            currentPatrolPoint = currentHit.position;
            hasPatrolPoint = true;
        }
    }

    private void Chase()
    {
        if (navAgent == null || !navAgent.enabled || playerTransform == null)
            return;

        navAgent.isStopped = false;
        navAgent.speed = chaseSpeed - slowdown;
        navAgent.updateRotation = false;

        navAgent.SetDestination(playerTransform.position);

        Vector3 lookDirection = playerTransform.position - transform.position;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, chaseTurnSpeed * Time.deltaTime);
        }
    }

    private void Attack()
    {
        if (navAgent != null && navAgent.enabled)
        {
            navAgent.isStopped = true;
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, engagementRange);

        Gizmos.color = Color.magenta;
        if (attackPoint != null)
        {
            Gizmos.DrawWireSphere(attackPoint.position, meleeRange);
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}        