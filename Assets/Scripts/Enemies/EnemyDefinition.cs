using UnityEngine;

[CreateAssetMenu(menuName = "Game/Enemies/Enemy Definition")]
public class EnemyDefinition : ScriptableObject
{
    [Header("General")]
    public string enemyName;

    [Header("Movement")]
    public float moveSpeed = 4f;
    public float acceleration = 12f;
    [Tooltip("Distance to the target where the enemy stops chasing (relative to attack range)")]
    [Range(0.1f, 1f)] public float stoppingRangeFactor = 0.7f;

    [Header("Perception")]
    [Tooltip("Detection radius while unaware")]
    public float aggroRadius = 18f;
    [Tooltip("Detection radius once alerted (spawned by waves, nests, etc.)")]
    public float alertRadius = 80f;
    public LayerMask targetMask;
    public float scanInterval = 0.3f;

    [Header("Attack")]
    public EnemyAttackDefinition attack;
}
