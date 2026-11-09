using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 2f;

    [Header("Path")]
    [SerializeField] private Transform[] waypoints;   // Drag your scene waypoints here, in order

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer -= spawnInterval;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoint == null)
        {
            Debug.LogError("Spawner: Enemy Prefab or Spawn Point is not assigned!");
            enabled = false;
            return;
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        EnemyBase follower = enemy.GetComponent<EnemyBase>();
        if (follower != null)
        {
            follower.SetWaypoints(waypoints);
        }
        else
        {
            Debug.LogWarning("Spawner: the enemy prefab has no EnemyBase component!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnPoint == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(spawnPoint.position, 0.3f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(spawnPoint.position, spawnPoint.position + spawnPoint.forward * 2f);
    }
}