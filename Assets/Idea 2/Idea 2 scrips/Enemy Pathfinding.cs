using UnityEngine;
using System.Collections.Generic;

public class EnemyPathFollower : MonoBehaviour
{
    // Every live enemy, so towers can find them without searching the scene
    public static readonly List<EnemyPathFollower> All = new List<EnemyPathFollower>();

    [Header("Movement Settings")]
    public Transform[] waypoints;
    public float moveSpeed = 5f;
    public float arrivalThreshold = 0.2f;
    public bool loopPath = true;

    [Header("Health Settings")]
    public float maxHealth = 100f;

    private float currentHealth;
    private int currentWaypointIndex = 0;
    private CaptureSystem captureSystem;

    public float CurrentHealth => currentHealth;

    void OnEnable()
    {
        All.Add(this);
    }

    void OnDisable()
    {
        All.Remove(this);
    }

    // Called by the Spawner right after the enemy is created
    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
        currentWaypointIndex = 0;
    }

    void Start()
    {
        currentHealth = maxHealth;

        captureSystem = FindFirstObjectByType<CaptureSystem>();

        if (HasValidWaypoints())
        {
            transform.position = waypoints[currentWaypointIndex].position;
        }
    }

    void Update()
    {
        if (captureSystem != null && captureSystem.IsMinigameActive())
        {
            return;
        }

        if (!HasValidWaypoints()) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetWaypoint.position,
            moveSpeed * Time.deltaTime
        );

        Vector3 direction = targetWaypoint.position - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, moveSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, targetWaypoint.position) < arrivalThreshold)
        {
            SetNextWaypoint();
        }
    }

    bool HasValidWaypoints()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning($"{name}: waypoints array is null or empty", this);
            return false;
        }

        if (currentWaypointIndex >= waypoints.Length || waypoints[currentWaypointIndex] == null)
        {
            Debug.LogWarning($"{name}: waypoint {currentWaypointIndex} is missing/unassigned", this);
            return false;
        }

        return true;
    }

    public void TakeDamage(float amount)
    {
        // No damage while the capture minigame is running
        if (captureSystem != null && captureSystem.IsMinigameActive()) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0f);
        print($"Enemy took {amount} damage. Current health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        print($"hp reached 0.");
        Destroy(gameObject);
    }

    void SetNextWaypoint()
    {
        currentWaypointIndex++;

        if (currentWaypointIndex >= waypoints.Length)
        {
            if (loopPath)
            {
                currentWaypointIndex = 0;
            }
            else
            {
                currentWaypointIndex = waypoints.Length - 1;
            }
        }
    }
}