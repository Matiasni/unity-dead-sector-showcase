using System;
using UnityEngine;

[RequireComponent(typeof(AutoTurret))]
public class Drone : PoolableObject
{
    private AutoTurret turret;

    private Transform owner;
    private DroneAbilityDefinition definition;

    private float orbitAngle;
    private float expireTime;

    public float RemainingNormalized => definition != null && definition.lifeTime > 0f ? Mathf.Clamp01((expireTime - Time.time) / definition.lifeTime) : 0f;

    public event Action<Drone> OnExpired;

    private void Awake()
    {
        turret = GetComponent<AutoTurret>();
    }

    public void Initialize(Transform owner, DroneAbilityDefinition definition, IWeaponModifiers modifiers)
    {
        this.owner = owner;
        this.definition = definition;

        orbitAngle = 0f;
        expireTime = Time.time + definition.lifeTime;

        turret.Configure(definition.weapon, definition.detectionRadius, definition.targetMask, modifiers);
        UpdateOrbit(0f);
    }

    public override void OnDespawned()
    {
        owner = null;
        definition = null;
    }

    private void Update()
    {
        if (definition == null) return;

        if (owner == null || Time.time >= expireTime)
        {
            Expire();
            return;
        }

        UpdateOrbit(Time.deltaTime);
    }

    private void UpdateOrbit(float deltaTime)
    {
        orbitAngle += definition.orbitSpeed * deltaTime;

        float radians = orbitAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(radians), 0f, Mathf.Sin(radians)) * definition.orbitRadius;
        offset.y = definition.orbitHeight;

        transform.position = owner.position + offset;
    }

    private void Expire()
    {
        OnExpired?.Invoke(this);
        Release();
    }
}
