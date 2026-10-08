using UnityEngine;

public class DestroyTargetsObjective : Objective
{
    [SerializeField] private HealthBehaviour[] targets;

    private int destroyedCount;

    protected override void OnActivated()
    {
        destroyedCount = 0;

        foreach (var target in targets)
        {
            if (target.IsDead || !target.gameObject.activeInHierarchy)
                destroyedCount++;
            else
                target.OnDied += HandleTargetDestroyed;
        }

        TryComplete();
    }

    private void OnDestroy()
    {
        foreach (var target in targets)
        {
            if (target != null)
                target.OnDied -= HandleTargetDestroyed;
        }
    }

    private void HandleTargetDestroyed()
    {
        destroyedCount++;
        NotifyChanged();
        TryComplete();
    }

    private void TryComplete()
    {
        if (destroyedCount >= targets.Length)
            Complete();
    }

    protected override LocalizedMessage GetProgress() => new("{0}/{1}", destroyedCount, targets.Length);
}
