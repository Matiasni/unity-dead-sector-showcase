using UnityEngine;

[CreateAssetMenu(menuName = "Game/Enemies/Attacks/Ranged")]
public class RangedAttackDefinition : EnemyAttackDefinition
{
    [Header("Ranged")]
    public WeaponSettings weapon;
    [Tooltip("Seconds aiming before opening fire")]
    public float aimTime = 0.6f;
    [Tooltip("Seconds the burst lasts")]
    public float burstDuration = 1f;
    [Tooltip("Layers that block line of sight")]
    public LayerMask lineOfSightMask = 1;

    public override IEnemyAttack CreateAttack(EnemyContext context)
    {
        return new RangedAttack(this, context);
    }
}
