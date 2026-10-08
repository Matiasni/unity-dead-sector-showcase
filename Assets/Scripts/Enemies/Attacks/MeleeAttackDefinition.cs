using UnityEngine;

[CreateAssetMenu(menuName = "Game/Enemies/Attacks/Melee")]
public class MeleeAttackDefinition : EnemyAttackDefinition
{
    [Header("Melee")]
    public int damage = 12;
    [Tooltip("Seconds the enemy telegraphs before hitting")]
    public float windupTime = 0.45f;
    public float recoveryTime = 0.5f;
    [Tooltip("Max distance at the moment of the hit")]
    public float hitRange = 2.2f;

    public override IEnemyAttack CreateAttack(EnemyContext context)
    {
        return new MeleeAttack(this, context);
    }
}
