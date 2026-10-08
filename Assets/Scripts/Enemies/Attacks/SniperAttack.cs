using UnityEngine;

public class SniperAttack : EnemyAttack<SniperAttackDefinition>
{
    private enum Phase { Tracking, Locked, Recovery }

    private const float LaserRange = 60f;

    private readonly Weapon weapon;

    private Phase phase;
    private float timer;
    private Vector3 lockedPoint;

    public SniperAttack(SniperAttackDefinition definition, EnemyContext context) : base(definition, context)
    {
        weapon = new Weapon(definition.weapon, context.WeaponView);
    }

    public override bool CanStart(Transform target)
    {
        return base.CanStart(target) && HasLineOfSight(AimPoint(target));
    }

    protected override void OnBegin(Transform target)
    {
        phase = Phase.Tracking;
        timer = definition.aimTime;

        context.Motor.Stop();
        context.Telegraph?.Show(definition.telegraphColor);
    }

    protected override void OnTick(Transform target, float deltaTime)
    {
        timer -= deltaTime;

        switch (phase)
        {
            case Phase.Tracking:
                Track(target, deltaTime);
                break;

            case Phase.Locked:
                DrawLaser(lockedPoint, definition.lockedColor);
                if (timer <= 0f) Fire();
                break;

            case Phase.Recovery:
                if (timer <= 0f) Finish();
                break;
        }
    }

    protected override void OnCancel()
    {
        context.AimIndicator?.Hide();
        weapon.ReleaseTrigger();
    }

    private void Track(Transform target, float deltaTime)
    {
        Vector3 aimPoint = AimPoint(target);

        context.Motor.FaceTowards(target.position, deltaTime);
        AimWeapon(aimPoint);
        DrawLaser(aimPoint, definition.trackingColor);

        if (timer > 0f) return;

        if (!HasLineOfSight(aimPoint))
        {
            Recover();
            return;
        }

        phase = Phase.Locked;
        timer = definition.lockTime;
        lockedPoint = aimPoint;
    }

    private void Fire()
    {
        AimWeapon(lockedPoint);

        weapon.ReleaseTrigger();
        weapon.SetTrigger(true);
        weapon.SetTrigger(false);

        Recover();
    }

    private void Recover()
    {
        context.AimIndicator?.Hide();
        context.Telegraph?.Hide();

        phase = Phase.Recovery;
        timer = definition.recoveryTime;
    }

    private void AimWeapon(Vector3 point)
    {
        Transform gun = context.WeaponView.transform;
        Vector3 direction = point - gun.position;

        if (direction.sqrMagnitude > 0.01f)
            gun.rotation = Quaternion.LookRotation(direction);
    }

    private void DrawLaser(Vector3 point, Color color)
    {
        Vector3 origin = context.WeaponView.Muzzle.position;
        Vector3 direction = (point - origin).normalized;
        Vector3 end = origin + direction * LaserRange;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, LaserRange, ~0, QueryTriggerInteraction.Ignore))
            end = hit.point;

        context.AimIndicator?.Show(origin, end, color);
    }

    private bool HasLineOfSight(Vector3 point)
    {
        return !Physics.Linecast(context.WeaponView.Muzzle.position, point, definition.lineOfSightMask, QueryTriggerInteraction.Ignore);
    }

    private static Vector3 AimPoint(Transform target) => target.position + Vector3.up;
}
