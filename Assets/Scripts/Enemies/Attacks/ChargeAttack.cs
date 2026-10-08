using UnityEngine;

public class ChargeAttack : EnemyAttack<ChargeAttackDefinition>
{
    private enum Phase { Windup, Charging, Stunned, Recovery }

    private const float CrashThreshold = 0.3f;

    private readonly IDamageModifier stunVulnerability;

    private Phase phase;
    private float timer;

    private Vector3 chargeDirection;
    private float traveledDistance;
    private bool hasHitTarget;

    public ChargeAttack(ChargeAttackDefinition definition, EnemyContext context) : base(definition, context)
    {
        stunVulnerability = new DamageMultiplier(definition.stunDamageMultiplier);
    }

    public override bool CanStart(Transform target)
    {
        return base.CanStart(target) && DistanceTo(target) >= definition.minRange;
    }

    protected override void OnBegin(Transform target)
    {
        phase = Phase.Windup;
        timer = definition.windupTime;

        context.Motor.Stop();
        context.Telegraph?.Show(definition.telegraphColor);
    }

    protected override void OnTick(Transform target, float deltaTime)
    {
        timer -= deltaTime;

        switch (phase)
        {
            case Phase.Windup:
                context.Motor.FaceTowards(target.position, deltaTime);
                if (timer <= 0f) StartCharge(target);
                break;

            case Phase.Charging:
                UpdateCharge(target, deltaTime);
                break;

            case Phase.Stunned:
                if (timer <= 0f) EndStun();
                break;

            case Phase.Recovery:
                if (timer <= 0f) Finish();
                break;
        }
    }

    protected override void OnCancel()
    {
        context.DamageHost?.RemoveDamageModifier(stunVulnerability);
    }

    private void StartCharge(Transform target)
    {
        phase = Phase.Charging;
        chargeDirection = DirectionTo(target);
        traveledDistance = 0f;
        hasHitTarget = false;

        context.Telegraph?.Hide();
    }

    private void UpdateCharge(Transform target, float deltaTime)
    {
        float step = definition.chargeSpeed * deltaTime;
        float moved = context.Motor.MoveDirect(chargeDirection * step);

        traveledDistance += moved;
        context.Motor.FaceTowards(context.Self.position + chargeDirection, deltaTime);

        if (!hasHitTarget && DistanceTo(target) <= definition.hitRadius)
            HitTarget(target);

        if (moved < step * CrashThreshold)
            Stun();
        else if (traveledDistance >= definition.maxChargeDistance)
            Recover();
    }

    private void HitTarget(Transform target)
    {
        hasHitTarget = true;

        DealDamage(target, definition.damage);

        target.GetComponentInParent<IKnockbackReceiver>()?.ApplyKnockback(
            chargeDirection * definition.knockbackSpeed,
            definition.knockbackDuration
        );
    }

    private void Stun()
    {
        phase = Phase.Stunned;
        timer = definition.stunDuration;

        context.DamageHost?.AddDamageModifier(stunVulnerability);
        context.Telegraph?.Show(definition.stunColor);
    }

    private void EndStun()
    {
        context.DamageHost?.RemoveDamageModifier(stunVulnerability);
        Finish();
    }

    private void Recover()
    {
        phase = Phase.Recovery;
        timer = definition.recoveryTime;
    }
}
