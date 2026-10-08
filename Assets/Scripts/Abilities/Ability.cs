using UnityEngine;

public abstract class Ability<TDefinition> : IAbility where TDefinition : AbilityDefinition
{
    protected readonly TDefinition definition;
    protected readonly AbilityContext context;

    private int availableUses;
    private float rechargeRemaining;

    public AbilityDefinition Definition => definition;
    public virtual bool IsReady => availableUses > 0 && CanAffordCost;
    public float CooldownNormalized => availableUses > 0 || Cooldown <= 0f ? 0f : rechargeRemaining / Cooldown;
    public virtual float ActiveProgress => -1f;

    public bool HasCharges => definition.HasCost;
    public int Charges => HasCharges && context.Wallet != null ? context.Wallet.Get(definition.costResource) / definition.costAmount : 0;
    public int MaxCharges => HasCharges && context.Wallet != null ? context.Wallet.GetCapacity(definition.costResource) / definition.costAmount : 0;

    protected virtual int MaxUses => 1;
    protected IStatProvider Stats => context.Stats;

    private float Cooldown => Stats.Apply(StatType.AbilityCooldown, definition.cooldown);
    private bool CanAffordCost => !definition.HasCost || (context.Wallet != null && context.Wallet.Has(definition.costResource, definition.costAmount));

    protected Ability(TDefinition definition, AbilityContext context)
    {
        this.definition = definition;
        this.context = context;

        availableUses = MaxUses;
    }

    public bool TryActivate()
    {
        if (!IsReady) return false;

        if (definition.HasCost && !context.Wallet.TrySpend(definition.costResource, definition.costAmount))
            return false;

        Activate();

        if (availableUses == MaxUses)
            rechargeRemaining = Cooldown;

        availableUses--;
        return true;
    }

    public virtual void Tick(float deltaTime)
    {
        int maxUses = MaxUses;

        if (availableUses >= maxUses)
        {
            availableUses = maxUses;
            return;
        }

        rechargeRemaining -= deltaTime;

        if (rechargeRemaining > 0f) return;

        availableUses = Mathf.Min(availableUses + 1, maxUses);

        if (availableUses < maxUses)
            rechargeRemaining = Cooldown;
    }

    public virtual void Dispose() { }

    protected abstract void Activate();
}
