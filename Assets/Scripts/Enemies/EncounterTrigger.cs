using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EncounterTrigger : MonoBehaviour
{
    [Serializable]
    public struct EncounterSpawn
    {
        public Enemy enemy;
        public Transform point;
        public PatrolRoute patrol;
        public bool alerted;
    }

    [SerializeField] private EncounterSpawn[] spawns;
    [SerializeField] private LayerMask playerMask;

    private bool hasTriggered;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered || (playerMask.value & (1 << other.gameObject.layer)) == 0) return;

        hasTriggered = true;

        foreach (var spawn in spawns)
            Spawn(spawn);
    }

    private static void Spawn(EncounterSpawn spawn)
    {
        var enemy = PoolManager.Spawn(spawn.enemy, spawn.point.position, spawn.point.rotation);

        if (!enemy.TryGetComponent(out EnemyBrain brain)) return;

        if (spawn.patrol != null)
            brain.AssignPatrol(spawn.patrol);

        if (spawn.alerted)
            brain.Alert();
    }

    private void OnDrawGizmos()
    {
        var color = hasTriggered ? Color.gray : SpawnerGizmos.EncounterColor;

        SpawnerGizmos.DrawArea(transform, GetComponent<BoxCollider>(), color);
        SpawnerGizmos.DrawLabel(transform.position + Vector3.up * 2.5f, color, $"Encounter: {name}");

        if (spawns == null) return;

        foreach (var spawn in spawns)
        {
            if (spawn.point == null) continue;

            string label = spawn.enemy != null ? spawn.enemy.name : "None";

            if (spawn.patrol != null)
                label += " (patrol)";

            SpawnerGizmos.DrawSpawnPoint(transform.position, spawn.point.position, color, 0.5f, label);
        }
    }
}
