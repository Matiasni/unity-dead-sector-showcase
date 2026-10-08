using UnityEngine;

[RequireComponent(typeof(HealthBehaviour))]
public class TrainingDummy : PoolableObject
{
    private HealthBehaviour health;

    public HealthBehaviour Health => health;

    private void Awake()
    {
        health = GetComponent<HealthBehaviour>();
    }

    public override void OnSpawned()
    {
        health.ResetHealth();
    }
}
