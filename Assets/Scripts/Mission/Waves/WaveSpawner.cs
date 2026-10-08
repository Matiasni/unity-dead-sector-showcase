using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnSpread = 2f;

    private readonly List<Enemy> alive = new();

    private WaveDefinition wave;
    private float duration;
    private float elapsed;
    private float nextSpawnTime;

    public bool IsRunning { get; private set; }

    public void Begin(WaveDefinition wave, float duration)
    {
        this.wave = wave;
        this.duration = duration;

        elapsed = 0f;
        nextSpawnTime = Time.time + wave.initialDelay;
        IsRunning = true;
    }

    public void Stop()
    {
        IsRunning = false;
    }

    private void Update()
    {
        if (!IsRunning) return;

        elapsed += Time.deltaTime;

        if (elapsed >= duration)
        {
            Stop();
            return;
        }

        if (Time.time < nextSpawnTime) return;

        float progress = Mathf.Clamp01(elapsed / duration);
        nextSpawnTime = Time.time + wave.GetInterval(progress);

        SpawnBatch(wave.GetBatchSize(progress));
    }

    private void SpawnBatch(int count)
    {
        alive.RemoveAll(enemy => enemy == null || !enemy.IsSpawned);

        for (int i = 0; i < count && alive.Count < wave.maxAlive; i++)
        {
            var prefab = wave.PickEnemy();

            if (prefab == null) return;

            var enemy = PoolManager.Spawn(prefab, GetSpawnPosition(), Quaternion.identity);
            AlertEnemy(enemy);
            alive.Add(enemy);
        }
    }

    private static void AlertEnemy(Enemy enemy)
    {
        if (enemy.TryGetComponent(out EnemyBrain brain))
            brain.Alert();
    }

    private Vector3 GetSpawnPosition()
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector2 offset = Random.insideUnitCircle * spawnSpread;

        return point.position + new Vector3(offset.x, 0f, offset.y);
    }

    private void OnDrawGizmos()
    {
        var color = IsRunning ? Color.red : SpawnerGizmos.WaveColor;

        SpawnerGizmos.DrawMarker(transform.position, color, $"Wave Spawner: {name}");

        if (spawnPoints == null) return;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i] != null)
                SpawnerGizmos.DrawSpawnPoint(transform.position, spawnPoints[i].position, color, spawnSpread, $"Wave point {i + 1}");
        }
    }
}
