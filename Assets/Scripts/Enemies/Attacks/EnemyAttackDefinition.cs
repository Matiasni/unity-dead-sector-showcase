using UnityEngine;

public abstract class EnemyAttackDefinition : ScriptableObject
{
    [Header("General")]
    public float attackRange = 2f;
    [Min(0f), Tooltip("Seconds between attacks")]
    public float cooldown = 1f;
    public Color telegraphColor = new(1f, 0.2f, 0.1f);

    public abstract IEnemyAttack CreateAttack(EnemyContext context);
}
