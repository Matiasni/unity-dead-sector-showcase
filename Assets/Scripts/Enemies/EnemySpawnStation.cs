using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemySpawnStation : MonoBehaviour, IInteractable
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private string displayName;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField, Min(1)] private int spawnCount = 3;
    [SerializeField] private float spawnSpread = 1.5f;
    [SerializeField] private float cooldown = 1.5f;

    private float readyTime;

    public LocalizedMessage InteractionPrompt => new("[F] Spawn {0} x{1}", displayName, spawnCount);

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    public void Interact(GameObject interactor)
    {
        if (Time.time < readyTime)
        {
            GameEvents.OnNotification("Spawner recharging");
            return;
        }

        readyTime = Time.time + cooldown;

        for (int i = 0; i < spawnCount; i++)
            Spawn(spawnPoints[i % spawnPoints.Length]);

        GameEvents.OnNotification("{0} x{1} incoming", displayName, spawnCount);
    }

    private void Spawn(Transform point)
    {
        Vector2 offset = Random.insideUnitCircle * spawnSpread;
        Vector3 position = point.position + new Vector3(offset.x, 0f, offset.y);

        var enemy = PoolManager.Spawn(enemyPrefab, position, point.rotation);

        if (enemy.TryGetComponent(out EnemyBrain brain))
            brain.Alert();
    }

    private void OnDrawGizmos()
    {
        string target = enemyPrefab != null ? enemyPrefab.name : "None";

        SpawnerGizmos.DrawLabel(transform.position + Vector3.up * 3f, SpawnerGizmos.StationColor, $"Spawn Station: {target} x{spawnCount}");

        if (spawnPoints == null) return;

        foreach (var point in spawnPoints)
        {
            if (point != null)
                SpawnerGizmos.DrawSpawnPoint(transform.position, point.position, SpawnerGizmos.StationColor, spawnSpread);
        }
    }
}
