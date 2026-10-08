using System.Collections.Generic;
using UnityEngine;

public class PeriodicSpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private float interval = 6f;
    [SerializeField, Min(1)] private int maxAlive = 4;
    [SerializeField] private float spawnRadius = 2.5f;

    [Header("Activation")]
    [SerializeField] private float activationRadius = 25f;
    [SerializeField] private LayerMask activatorMask;

    private readonly List<Enemy> alive = new();
    private readonly Collider[] activatorBuffer = new Collider[1];

    private float nextSpawnTime;

    private void OnEnable()
    {
        nextSpawnTime = Time.time + interval;
    }

    private void Update()
    {
        if (Time.time < nextSpawnTime) return;

        nextSpawnTime = Time.time + interval;

        if (!IsActivatorNearby()) return;

        alive.RemoveAll(enemy => enemy == null || !enemy.IsSpawned);

        if (alive.Count >= maxAlive) return;

        var enemy = PoolManager.Spawn(enemyPrefab, GetSpawnPosition(), transform.rotation);

        if (enemy.TryGetComponent(out EnemyBrain brain))
            brain.Alert();

        alive.Add(enemy);
    }

    private bool IsActivatorNearby()
    {
        return Physics.OverlapSphereNonAlloc(transform.position, activationRadius, activatorBuffer, activatorMask, QueryTriggerInteraction.Ignore) > 0;
    }

    private Vector3 GetSpawnPosition()
    {
        Vector2 direction = Random.insideUnitCircle.normalized * spawnRadius;
        return transform.position + new Vector3(direction.x, 0f, direction.y);
    }

    private void OnDrawGizmos()
    {
        string target = enemyPrefab != null ? enemyPrefab.name : "None";

        SpawnerGizmos.DrawMarker(transform.position, SpawnerGizmos.NestColor, $"Nest Spawner ({target} every {interval}s, max {maxAlive})");

        Gizmos.color = SpawnerGizmos.NestColor;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.3f, 0.3f, 0.4f);
        Gizmos.DrawWireSphere(transform.position, activationRadius);
    }
}
