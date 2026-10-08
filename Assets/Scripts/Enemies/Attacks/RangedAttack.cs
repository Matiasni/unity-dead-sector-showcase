using UnityEngine;

public class RangedAttack : EnemyAttack<RangedAttackDefinition>
{
    private enum Phase { Aim, Fire }

    private readonly Weapon weapon;

    private Phase phase;
    private float timer;

    public RangedAttack(RangedAttackDefinition definition, EnemyContext context) : base(definition, context)
    {
        weapon = new Weapon(definition.weapon, context.WeaponView);
    }

    public override bool CanStart(Transform target)
    {
        return base.CanStart(target) && HasLineOfSight(target);
    }

    protected override void OnBegin(Transform target)
    {
        phase = Phase.Aim;
        timer = definition.aimTime;

        context.Motor.Stop();
        context.Telegraph?.Show(definition.telegraphColor);
    }

    protected override void OnTick(Transform target, float deltaTime)
    {
        context.Motor.FaceTowards(target.position, deltaTime);
        AimWeapon(target);
        timer -= deltaTime;

        if (phase == Phase.Aim)
        {
            if (timer > 0f) return;

            phase = Phase.Fire;
            timer = definition.burstDuration;
            context.Telegraph?.Hide();
            return;
        }

        weapon.SetTrigger(true);

        if (timer <= 0f)
            EndBurst();
    }

    protected override void OnCancel()
    {
        weapon.ReleaseTrigger();
    }

    private void EndBurst()
    {
        weapon.SetTrigger(false);
        Finish();
    }

    private void AimWeapon(Transform target)
    {
        Transform gun = context.WeaponView.transform;
        Vector3 direction = target.position + Vector3.up - gun.position;

        if (direction.sqrMagnitude > 0.01f)
            gun.rotation = Quaternion.LookRotation(direction);
    }

    private bool HasLineOfSight(Transform target)
    {
        Vector3 origin = context.WeaponView.Muzzle.position;
        Vector3 destination = target.position + Vector3.up;

        return !Physics.Linecast(origin, destination, definition.lineOfSightMask, QueryTriggerInteraction.Ignore);
    }
}
