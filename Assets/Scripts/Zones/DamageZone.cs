using UnityEngine;

public class DamageZone : TickZone<IDamageable>
{
    [SerializeField] private int damagePerTick = 10;

    protected override void Apply(IDamageable target)
    {
        target.TakeDamage(damagePerTick);
    }
}
