using UnityEngine;

[RequireComponent(typeof(HealthBehaviour))]
public class DestroyOnDeath : MonoBehaviour
{
    [SerializeField] private float delay;

    private HealthBehaviour health;

    private void Awake()
    {
        health = GetComponent<HealthBehaviour>();
    }

    private void OnEnable()
    {
        health.OnDied += DestroySelf;
    }

    private void OnDisable()
    {
        health.OnDied -= DestroySelf;
    }

    private void DestroySelf()
    {
        Destroy(gameObject, delay);
    }
}
