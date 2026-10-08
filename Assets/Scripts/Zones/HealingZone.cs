using UnityEngine;

public class HealingZone : TickZone<IHealable>
{
    [SerializeField] private int healPerTick = 5;

    protected override void Apply(IHealable target)
    {
        target.Heal(healPerTick);
    }
}
