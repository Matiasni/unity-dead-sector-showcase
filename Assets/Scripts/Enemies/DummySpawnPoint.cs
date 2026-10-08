using System.Collections;
using UnityEngine;

public class DummySpawnPoint : MonoBehaviour
{
    [SerializeField] private TrainingDummy dummyPrefab;
    [SerializeField] private float respawnDelay = 3f;

    private TrainingDummy current;

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        current = PoolManager.Spawn(dummyPrefab, transform.position, transform.rotation);
        current.Health.OnDied += HandleDeath;
    }

    private void HandleDeath()
    {
        current.Health.OnDied -= HandleDeath;
        current.Release();
        current = null;

        StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        Spawn();
    }

    private void OnDestroy()
    {
        if (current != null)
            current.Health.OnDied -= HandleDeath;
    }

    private void OnDrawGizmos()
    {
        string target = dummyPrefab != null ? dummyPrefab.name : "None";
        SpawnerGizmos.DrawMarker(transform.position, SpawnerGizmos.DummyColor, $"Dummy Spawn ({target}, {respawnDelay}s)");
    }
}
