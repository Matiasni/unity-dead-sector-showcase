using UnityEngine;

public class DashAbility : Ability<DashAbilityDefinition>
{
    private readonly IDashMotor motor;
    private readonly ICombatInterruptible combat;
    private readonly IDamageModifierHost damageHost;
    private readonly IDamageModifier immunity = new DamageImmunity();

    private float invulnerableUntil;
    private bool isInvulnerable;

    public DashAbility(DashAbilityDefinition definition, AbilityContext context) : base(definition, context)
    {
        motor = context.Owner.GetComponent<IDashMotor>();
        combat = context.Owner.GetComponent<ICombatInterruptible>();
        damageHost = context.Owner.GetComponent<IDamageModifierHost>();
    }

    public override bool IsReady => base.IsReady && motor != null;

    protected override int MaxUses => 1 + Mathf.RoundToInt(Stats.Flat(StatType.DashCharges));

    protected override void Activate()
    {
        Vector3 direction = motor.MoveDirection.sqrMagnitude > 0.01f
            ? motor.MoveDirection
            : context.Owner.transform.forward;

        combat?.Interrupt(definition.combatLockDuration);
        motor.Dash(direction, definition.speed, definition.duration);

        StartInvulnerability();
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        if (isInvulnerable && Time.time >= invulnerableUntil)
            EndInvulnerability();
    }

    public override void Dispose()
    {
        EndInvulnerability();
    }

    private void StartInvulnerability()
    {
        if (damageHost == null || definition.invulnerabilityDuration <= 0f) return;

        invulnerableUntil = Time.time + definition.invulnerabilityDuration;
        isInvulnerable = true;
        damageHost.AddDamageModifier(immunity);
    }

    private void EndInvulnerability()
    {
        if (!isInvulnerable) return;

        isInvulnerable = false;
        damageHost?.RemoveDamageModifier(immunity);
    }
}
