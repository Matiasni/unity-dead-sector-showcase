using UnityEngine;

public class TargetScanner
{
    private readonly Collider[] buffer = new Collider[32];

    private float radius;
    private LayerMask targetMask;

    public TargetScanner(float radius, LayerMask targetMask)
    {
        Configure(radius, targetMask);
    }

    public void Configure(float radius, LayerMask targetMask)
    {
        this.radius = radius;
        this.targetMask = targetMask;
    }

    public Collider FindClosest(Vector3 origin)
    {
        int count = Physics.OverlapSphereNonAlloc(origin, radius, buffer, targetMask, QueryTriggerInteraction.Ignore);

        Collider closest = null;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            if (buffer[i].GetComponentInParent<IDamageable>() == null) continue;

            var living = buffer[i].GetComponentInParent<ILivingTarget>();
            if (living != null && !living.IsAlive) continue;

            float distance = (buffer[i].bounds.center - origin).sqrMagnitude;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = buffer[i];
            }
        }

        return closest;
    }
}
