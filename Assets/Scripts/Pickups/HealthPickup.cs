using UnityEngine;

public class HealthPickup : Pickup
{
    [SerializeField] private int healAmount = 25;

    protected override bool TryApply(Collider other)
    {
        var healable = other.GetComponentInParent<IHealable>();

        if (healable == null || !healable.CanHeal) return false;

        healable.Heal(healAmount);
        return true;
    }
}
