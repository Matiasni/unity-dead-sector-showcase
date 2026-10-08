using UnityEngine;

[RequireComponent(typeof(HealthBehaviour))]
public class Enemy : PoolableObject
{
    private HealthBehaviour health;
    private IEnemyMotor motor;

    public HealthBehaviour Health => health;

    private void Awake()
    {
        health = GetComponent<HealthBehaviour>();
        motor = GetComponent<IEnemyMotor>();
    }

    private void OnEnable()
    {
        health.OnDied += Release;
    }

    private void OnDisable()
    {
        health.OnDied -= Release;
    }

    public override void OnSpawned()
    {
        health.ResetHealth();
        motor?.Warp(transform.position);
    }
}
