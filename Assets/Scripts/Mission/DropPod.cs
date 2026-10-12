using System.Collections.Generic;
using UnityEngine;

public class DropPod : PoolableObject
{
    private static readonly Collider[] impactBuffer = new Collider[32];

    [SerializeField] private Transform visual;
    [SerializeField] private float dropHeight = 35f;
    [SerializeField] private float dropDuration = 0.8f;
    [SerializeField] private float lingerTime = 4f;

    [Header("Impact")]
    [SerializeField] private int impactDamage = 100;
    [SerializeField] private float impactRadius = 3f;
    [SerializeField] private LayerMask impactMask;

    [Header("Feedback")]
    [SerializeField] private SoundEffect landSound;
    [SerializeField, Range(0f, 1f)] private float landShake = 0.6f;
    [SerializeField] private ParticleSystem[] trailSystems;
    [SerializeField] private PooledEffect impactEffect;

    private readonly HashSet<IDamageable> damagedTargets = new();

    private float elapsed;
    private bool hasLanded;

    public float DropDuration => dropDuration;

    public override void OnSpawned()
    {
        elapsed = 0f;
        hasLanded = false;
        UpdateVisual(0f);

        foreach (var trail in trailSystems)
        {
            trail.Clear(false);
            trail.Play(false);
        }
    }

    private void Update()
    {
        elapsed += Time.deltaTime;

        if (!hasLanded)
        {
            float t = Mathf.Clamp01(elapsed / dropDuration);
            UpdateVisual(t);

            if (t >= 1f)
                Land();

            return;
        }

        if (elapsed >= dropDuration + lingerTime)
            Release();
    }

    private void Land()
    {
        hasLanded = true;

        SoundPlayer.Play(landSound, transform.position);
        GameEvents.OnCameraShake(landShake);

        foreach (var trail in trailSystems)
            trail.Stop(false, ParticleSystemStopBehavior.StopEmitting);

        if (impactEffect != null)
            PoolManager.Spawn(impactEffect, transform.position, Quaternion.identity);

        int count = Physics.OverlapSphereNonAlloc(transform.position, impactRadius, impactBuffer, impactMask, QueryTriggerInteraction.Ignore);

        damagedTargets.Clear();

        for (int i = 0; i < count; i++)
        {
            var damageable = impactBuffer[i].GetComponentInParent<IDamageable>();

            if (damageable != null && damagedTargets.Add(damageable))
                damageable.TakeDamage(impactDamage);
        }
    }

    private void UpdateVisual(float t)
    {
        visual.localPosition = Vector3.up * (dropHeight * (1f - t * t));
    }
}
