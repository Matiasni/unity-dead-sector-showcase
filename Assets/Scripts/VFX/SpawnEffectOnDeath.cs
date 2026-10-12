using UnityEngine;

[RequireComponent(typeof(HealthBehaviour))]
public class SpawnEffectOnDeath : MonoBehaviour
{
    [SerializeField] private PooledEffect effect;
    [SerializeField] private PooledEffect remainsEffect;
    [SerializeField] private LayerMask groundMask = 1;
    [SerializeField] private float groundSearchDistance = 4f;

    private HealthBehaviour health;

    private void Awake()
    {
        health = GetComponent<HealthBehaviour>();
    }

    private void OnEnable()
    {
        health.OnDied += Spawn;
    }

    private void OnDisable()
    {
        health.OnDied -= Spawn;
    }

    private void Spawn()
    {
        Vector3 position = transform.position;

        if (Physics.Raycast(position + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, groundSearchDistance, groundMask, QueryTriggerInteraction.Ignore))
            position = hit.point;

        PoolManager.Spawn(effect, position, Quaternion.identity);

        if (remainsEffect != null)
            PoolManager.Spawn(remainsEffect, position, Quaternion.identity);
    }
}
