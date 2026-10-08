using UnityEngine;

public class HealAbility : Ability<HealAbilityDefinition>
{
    private readonly IHealable healable;

    private float remainingTime;
    private float pendingHeal;
    private int healedSoFar;

    public HealAbility(HealAbilityDefinition definition, AbilityContext context) : base(definition, context)
    {
        healable = context.Owner.GetComponent<IHealable>();
    }

    private bool IsHealing => remainingTime > 0f;

    public override bool IsReady => base.IsReady && !IsHealing && healable != null && healable.CanHeal;
    public override float ActiveProgress => IsHealing && definition.healDuration > 0f ? remainingTime / definition.healDuration : -1f;

    protected override void Activate()
    {
        if (definition.healDuration <= 0f)
        {
            healable.Heal(definition.healAmount);
            return;
        }

        remainingTime = definition.healDuration;
        pendingHeal = 0f;
        healedSoFar = 0;
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        if (!IsHealing) return;

        float step = Mathf.Min(deltaTime, remainingTime);
        remainingTime -= step;
        pendingHeal += definition.healAmount * (step / definition.healDuration);

        int amount = Mathf.FloorToInt(pendingHeal);

        if (!IsHealing)
            amount = definition.healAmount - healedSoFar;

        if (amount <= 0) return;

        pendingHeal -= amount;
        healedSoFar += amount;
        healable.Heal(amount);
    }
}
