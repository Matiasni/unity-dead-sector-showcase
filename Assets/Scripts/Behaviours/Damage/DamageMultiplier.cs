using UnityEngine;

public class DamageMultiplier : IDamageModifier
{
    private readonly float multiplier;

    public DamageMultiplier(float multiplier)
    {
        this.multiplier = multiplier;
    }

    public int ModifyDamage(int amount) => Mathf.RoundToInt(amount * multiplier);
}
