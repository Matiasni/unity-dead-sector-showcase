public class DroneAbility : Ability<DroneAbilityDefinition>
{
    private Drone activeDrone;

    public DroneAbility(DroneAbilityDefinition definition, AbilityContext context) : base(definition, context) { }

    public override bool IsReady => base.IsReady && activeDrone == null;
    public override float ActiveProgress => activeDrone != null ? activeDrone.RemainingNormalized : -1f;

    protected override void Activate()
    {
        var owner = context.Owner.transform;

        activeDrone = PoolManager.Spawn(definition.dronePrefab, owner.position, owner.rotation);
        activeDrone.OnExpired += HandleDroneExpired;
        activeDrone.Initialize(owner, definition, WeaponStatModifiers.ForDrone(Stats));
    }

    public override void Dispose()
    {
        if (activeDrone == null) return;

        var drone = activeDrone;
        HandleDroneExpired(drone);
        drone.Release();
    }

    private void HandleDroneExpired(Drone drone)
    {
        drone.OnExpired -= HandleDroneExpired;
        activeDrone = null;
    }
}
