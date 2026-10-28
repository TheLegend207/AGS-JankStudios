using UnityEngine;

public class EnemyPathFollower : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform[] waypoints;
    public float moveSpeed = 5f;
    public float arrivalThreshold = 0.2f;
    public bool loopPath = true; 

    private int currentWaypointIndex = 0;

    void Start()
    {
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[currentWaypointIndex].position;
        }
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

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
