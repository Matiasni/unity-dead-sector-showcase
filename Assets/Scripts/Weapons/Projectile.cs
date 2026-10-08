using System.Collections.Generic;
using UnityEngine;

public class Projectile : PoolableObject
{
    private static readonly RaycastHit[] hitBuffer = new RaycastHit[16];
    private static readonly IComparer<RaycastHit> ByDistance = Comparer<RaycastHit>.Create((a, b) => a.distance.CompareTo(b.distance));

    [SerializeField] private float radius = 0.1f;

    private readonly HashSet<IDamageable> damagedTargets = new();

    private Vector3 velocity;
    private int damage;
    private int remainingPierce;
    private float expireTime;
    private int hitMask;

    private void Awake()
    {
        hitMask = BuildCollisionMask(gameObject.layer);
    }

    public void Initialize(Vector3 direction, float speed, int damage, float lifeTime, int pierceCount = 0)
    {
        velocity = direction.normalized * speed;
        this.damage = damage;
        remainingPierce = pierceCount;
        expireTime = Time.time + lifeTime;
    }

    public override void OnDespawned()
    {
        damagedTargets.Clear();
    }

    private void Update()
    {
        if (Time.time >= expireTime)
        {
            Release();
            return;
        }

        Vector3 step = velocity * Time.deltaTime;
        float distance = step.magnitude;

        if (distance <= 0f) return;

        if (Sweep(step / distance, distance)) return;

        transform.position += step;
    }

    private bool Sweep(Vector3 direction, float distance)
    {
        int count = Physics.SphereCastNonAlloc(transform.position, radius, direction, hitBuffer, distance, hitMask, QueryTriggerInteraction.Ignore);

        System.Array.Sort(hitBuffer, 0, count, ByDistance);

        for (int i = 0; i < count; i++)
        {
            if (HandleHit(hitBuffer[i]))
                return true;
        }

        return false;
    }

    private bool HandleHit(RaycastHit hit)
    {
        var damageable = hit.collider.GetComponentInParent<IDamageable>();

        if (damageable == null)
        {
            Release();
            return true;
        }

        if (!damagedTargets.Add(damageable)) return false;

        damageable.TakeDamage(damage);

        if (remainingPierce > 0)
        {
            remainingPierce--;
            return false;
        }

        Release();
        return true;
    }

    private static int BuildCollisionMask(int layer)
    {
        int mask = 0;

        for (int i = 0; i < 32; i++)
        {
            if (!Physics.GetIgnoreLayerCollision(layer, i))
                mask |= 1 << i;
        }

        return mask;
    }
}
