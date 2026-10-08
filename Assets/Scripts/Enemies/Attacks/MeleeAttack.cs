using UnityEngine;

public class MeleeAttack : EnemyAttack<MeleeAttackDefinition>
{
    private enum Phase { Windup, Recovery }

    private Phase phase;
    private float timer;

    public MeleeAttack(MeleeAttackDefinition definition, EnemyContext context) : base(definition, context) { }

    protected override void OnBegin(Transform target)
    {
        phase = Phase.Windup;
        timer = definition.windupTime;

        context.Motor.Stop();
        context.Telegraph?.Show(definition.telegraphColor);
    }

    protected override void OnTick(Transform target, float deltaTime)
    {
        timer -= deltaTime;

        if (phase == Phase.Windup)
        {
            context.Motor.FaceTowards(target.position, deltaTime);

            if (timer <= 0f)
                Strike(target);

            return;
        }

        if (timer <= 0f)
            Finish();
    }

    private void Strike(Transform target)
    {
        if (DistanceTo(target) <= definition.hitRange)
            DealDamage(target, definition.damage);

        context.Telegraph?.Hide();

        phase = Phase.Recovery;
        timer = definition.recoveryTime;
    }
}
