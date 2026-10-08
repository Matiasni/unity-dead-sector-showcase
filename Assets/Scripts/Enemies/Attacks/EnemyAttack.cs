using UnityEngine;

public abstract class EnemyAttack<TDefinition> : IEnemyAttack where TDefinition : EnemyAttackDefinition
{
    protected readonly TDefinition definition;
    protected readonly EnemyContext context;

    private float readyTime;

    public float Range => definition.attackRange;
    public bool IsBusy { get; private set; }

    protected EnemyAttack(TDefinition definition, EnemyContext context)
    {
        this.definition = definition;
        this.context = context;
    }

    public virtual bool CanStart(Transform target)
    {
        return !IsBusy && Time.time >= readyTime && target != null && DistanceTo(target) <= definition.attackRange;
    }

    public void Begin(Transform target)
    {
        IsBusy = true;
        OnBegin(target);
    }

    public void Tick(Transform target, float deltaTime)
    {
        if (!IsBusy) return;

        if (target == null)
        {
            Cancel();
            return;
        }

        OnTick(target, deltaTime);
    }

    public void Cancel()
    {
        if (!IsBusy) return;

        OnCancel();
        Finish();
    }

    protected void Finish()
    {
        IsBusy = false;
        readyTime = Time.time + definition.cooldown;
        context.Telegraph?.Hide();
    }

    protected float DistanceTo(Transform target)
    {
        Vector3 offset = target.position - context.Self.position;
        offset.y = 0f;
        return offset.magnitude;
    }

    protected Vector3 DirectionTo(Transform target)
    {
        Vector3 offset = target.position - context.Self.position;
        offset.y = 0f;
        return offset.sqrMagnitude > 0.001f ? offset.normalized : context.Self.forward;
    }

    protected static void DealDamage(Transform target, int damage)
    {
        target.GetComponentInParent<IDamageable>()?.TakeDamage(damage);
    }

    protected abstract void OnBegin(Transform target);
    protected abstract void OnTick(Transform target, float deltaTime);
    protected virtual void OnCancel() { }
}
