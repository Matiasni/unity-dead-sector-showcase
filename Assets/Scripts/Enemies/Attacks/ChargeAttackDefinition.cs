using UnityEngine;

[CreateAssetMenu(menuName = "Game/Enemies/Attacks/Charge")]
public class ChargeAttackDefinition : EnemyAttackDefinition
{
    [Header("Charge")]
    [Tooltip("Won't charge if the target is closer than this")]
    public float minRange = 3f;
    public float windupTime = 0.8f;
    public float chargeSpeed = 16f;
    public float maxChargeDistance = 14f;
    public float recoveryTime = 0.6f;

    [Header("Impact")]
    public int damage = 35;
    public float hitRadius = 1.6f;
    public float knockbackSpeed = 14f;
    public float knockbackDuration = 0.2f;

    [Header("Stun")]
    [Tooltip("Seconds stunned after crashing into an obstacle")]
    public float stunDuration = 2f;
    [Tooltip("Damage multiplier while stunned")]
    public float stunDamageMultiplier = 2f;
    public Color stunColor = new(1f, 0.9f, 0.2f);

    public override IEnemyAttack CreateAttack(EnemyContext context)
    {
        return new ChargeAttack(this, context);
    }
}
