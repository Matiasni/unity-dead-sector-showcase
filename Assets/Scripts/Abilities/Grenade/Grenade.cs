using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Grenade : PoolableObject
{
    private static readonly Collider[] hitBuffer = new Collider[64];

    private readonly HashSet<IDamageable> damagedTargets = new();

    private Rigidbody rb;
    private Collider grenadeCollider;

    private GrenadeSettings settings;
    private float explosionRadius;

    private Transform anchor;
    private Vector3 anchorLocalPosition;
    private Quaternion anchorLocalRotation;

    private float armedTime;
    private float explodeTime;
    private bool isArmed;

    public float ArmedProgress => isArmed ? Mathf.Clamp01((Time.time - armedTime) / settings.detonationDelay) : -1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grenadeCollider = GetComponent<Collider>();
    }

    public void Launch(Vector3 velocity, GrenadeSettings settings, float explosionRadius)
    {
        this.settings = settings;
        this.explosionRadius = explosionRadius;

        rb.linearVelocity = velocity;
        rb.angularVelocity = Random.insideUnitSphere * 10f;

        explodeTime = Time.time + settings.maxFlightTime;
    }

    public override void OnDespawned()
    {
        settings = null;
        anchor = null;
        isArmed = false;

        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        grenadeCollider.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (settings == null || isArmed) return;

        if (settings.sticky)
        {
            ContactPoint contact = collision.GetContact(0);
            StickTo(collision.collider.transform, contact.point, contact.normal);
        }

        Arm();
    }

    private void StickTo(Transform target, Vector3 point, Vector3 normal)
    {
        float radius = grenadeCollider.bounds.extents.x;

        rb.isKinematic = true;
        grenadeCollider.enabled = false;

        transform.position = point + normal * radius;

        anchor = target;
        anchorLocalPosition = anchor.InverseTransformPoint(transform.position);
        anchorLocalRotation = Quaternion.Inverse(anchor.rotation) * transform.rotation;
    }

    private void Arm()
    {
        isArmed = true;
        armedTime = Time.time;
        explodeTime = Time.time + settings.detonationDelay;
    }

    private void LateUpdate()
    {
        if (anchor == null || !anchor.gameObject.activeInHierarchy) return;

        transform.SetPositionAndRotation(
            anchor.TransformPoint(anchorLocalPosition),
            anchor.rotation * anchorLocalRotation
        );
    }

    private void Update()
    {
        if (settings == null || Time.time < explodeTime) return;

        Explode();
    }

    private void Explode()
    {
        Vector3 center = transform.position;

        DamageTargetsInRadius(center);

        var explosion = PoolManager.Spawn(settings.explosionPrefab, center, Quaternion.identity);
        explosion.Play(explosionRadius);

        SoundPlayer.Play(settings.explosionSound, center);
        GameEvents.OnCameraShake(settings.explosionShake);

        Release();
    }

    private void DamageTargetsInRadius(Vector3 center)
    {
        int count = Physics.OverlapSphereNonAlloc(center, explosionRadius, hitBuffer, settings.damageMask, QueryTriggerInteraction.Ignore);

        damagedTargets.Clear();

        for (int i = 0; i < count; i++)
        {
            var damageable = hitBuffer[i].GetComponentInParent<IDamageable>();

            if (damageable != null && damagedTargets.Add(damageable))
                damageable.TakeDamage(settings.damage);
        }
    }
}
