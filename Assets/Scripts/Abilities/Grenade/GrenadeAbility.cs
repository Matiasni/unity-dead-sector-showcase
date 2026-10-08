using UnityEngine;

public class GrenadeAbility : Ability<GrenadeAbilityDefinition>
{
    public GrenadeAbility(GrenadeAbilityDefinition definition, AbilityContext context) : base(definition, context) { }

    protected override void Activate()
    {
        Vector3 origin = context.CastPoint.position;
        Vector3 target = GetTargetPoint();

        var grenade = PoolManager.Spawn(definition.grenade.grenadePrefab, origin, Quaternion.identity);
        float radius = Stats.Apply(StatType.GrenadeRadius, definition.grenade.explosionRadius);
        grenade.Launch(CalculateLaunchVelocity(origin, target), definition.grenade, radius);
    }

    private Vector3 GetTargetPoint()
    {
        Vector3 ownerPosition = context.Owner.transform.position;
        Vector3 offset = context.AimProvider.GetAimPoint() - ownerPosition;
        offset.y = 0f;

        return ownerPosition + Vector3.ClampMagnitude(offset, definition.maxThrowDistance);
    }

    private Vector3 CalculateLaunchVelocity(Vector3 origin, Vector3 target)
    {
        float time = Mathf.Max(0.1f, definition.flightTime);

        return (target - origin) / time - 0.5f * time * Physics.gravity;
    }
}
