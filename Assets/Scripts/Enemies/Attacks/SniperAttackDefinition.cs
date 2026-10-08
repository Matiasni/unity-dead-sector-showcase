using UnityEngine;

[CreateAssetMenu(menuName = "Game/Enemies/Attacks/Sniper")]
public class SniperAttackDefinition : EnemyAttackDefinition
{
    [Header("Sniper")]
    public WeaponSettings weapon;
    [Tooltip("Seconds tracking the target with the laser")]
    public float aimTime = 2f;
    [Tooltip("Seconds the aim is locked before firing (dodge window)")]
    public float lockTime = 0.35f;
    public float recoveryTime = 0.6f;
    [Tooltip("Layers that block line of sight")]
    public LayerMask lineOfSightMask = 1;

    [Header("Laser")]
    public Color trackingColor = new(1f, 0.15f, 0.15f, 0.6f);
    public Color lockedColor = new(1f, 1f, 1f, 0.95f);

    public override IEnemyAttack CreateAttack(EnemyContext context)
    {
        return new SniperAttack(this, context);
    }
}
