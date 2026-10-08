using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Mission/Wave")]
public class WaveDefinition : ScriptableObject
{
    [Serializable]
    public struct WaveEntry
    {
        public Enemy enemy;
        [Min(0f)] public float weight;
    }

    [SerializeField] private WaveEntry[] enemies;

    [Header("Pacing")]
    public float initialDelay = 1f;
    [Tooltip("Seconds between spawns at the start and at the end of the wave")]
    public float startInterval = 5f;
    public float endInterval = 1.5f;
    [Min(1)] public int startBatchSize = 1;
    [Min(1)] public int endBatchSize = 3;
    [Min(1)] public int maxAlive = 15;

    public float GetInterval(float progress) => Mathf.Lerp(startInterval, endInterval, progress);

    public int GetBatchSize(float progress) => Mathf.RoundToInt(Mathf.Lerp(startBatchSize, endBatchSize, progress));

    public Enemy PickEnemy()
    {
        float total = 0f;

        foreach (var entry in enemies)
            total += entry.weight;

        float roll = UnityEngine.Random.value * total;

        foreach (var entry in enemies)
        {
            roll -= entry.weight;

            if (roll <= 0f)
                return entry.enemy;
        }

        return enemies.Length > 0 ? enemies[^1].enemy : null;
    }
}
