public class ShieldAbility : Ability<ShieldAbilityDefinition>
{
    private readonly IDamageModifierHost damageHost;

    private ForceShield activeShield;

    public ShieldAbility(ShieldAbilityDefinition definition, AbilityContext context) : base(definition, context)
    {
        damageHost = context.Owner.GetComponent<IDamageModifierHost>();
    }

    public override bool IsReady => base.IsReady && activeShield == null;
    public override float ActiveProgress => activeShield != null ? activeShield.DurabilityNormalized : -1f;

    protected override void Activate()
    {
        activeShield = PoolManager.Spawn(definition.shieldPrefab, context.Owner.transform);
        activeShield.OnCollapsed += HandleShieldCollapsed;
        activeShield.Initialize(definition, damageHost, Stats.ApplyRounded(StatType.ShieldDurability, definition.durability));
    }

    public override void Dispose()
    {
        if (activeShield == null) return;

        var shield = activeShield;
        HandleShieldCollapsed(shield);
        shield.Release();
    }

    private void HandleShieldCollapsed(ForceShield shield)
    {
        shield.OnCollapsed -= HandleShieldCollapsed;
        activeShield = null;
    }
}
