using System;
using UnityEngine;

public class ForceShield : PoolableObject, IDamageable, IDamageModifier
{
    private IDamageModifierHost host;

    private float expireTime;
    private int maxDurability;
    private int remainingDurability;
    private bool isActive;

    public float DurabilityNormalized => maxDurability > 0 ? (float)remainingDurability / maxDurability : 0f;

    public event Action<ForceShield> OnCollapsed;
    public event Action OnHit;

    public void Initialize(ShieldAbilityDefinition definition, IDamageModifierHost host, int durability)
    {
        transform.localPosition = definition.offset;
        transform.localScale = Vector3.one * definition.radius * 2f;

        expireTime = Time.time + definition.duration;
        maxDurability = durability;
        remainingDurability = durability;
        isActive = true;

        this.host = host;
        host?.AddDamageModifier(this);
    }

    public override void OnDespawned()
    {
        isActive = false;

        host?.RemoveDamageModifier(this);
        host = null;
    }

    public void TakeDamage(int amount)
    {
        Absorb(amount);
    }

    public int ModifyDamage(int amount)
    {
        return Absorb(amount);
    }

    private int Absorb(int amount)
    {
        if (!isActive || amount <= 0) return amount;

        int absorbed = Mathf.Min(amount, remainingDurability);
        remainingDurability -= absorbed;

        OnHit?.Invoke();

        if (remainingDurability <= 0)
            Collapse();

        return amount - absorbed;
    }

    private void Update()
    {
        if (isActive && Time.time >= expireTime)
            Collapse();
    }

    private void Collapse()
    {
        isActive = false;
        OnCollapsed?.Invoke(this);
        Release();
    }
}
