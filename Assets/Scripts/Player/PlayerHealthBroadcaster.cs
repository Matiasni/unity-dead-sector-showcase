using UnityEngine;

[RequireComponent(typeof(HealthBehaviour))]
public class PlayerHealthBroadcaster : MonoBehaviour
{
    private HealthBehaviour health;

    private void Awake()
    {
        health = GetComponent<HealthBehaviour>();
    }

    private void OnEnable()
    {
        health.OnHealthChanged += GameEvents.OnPlayerHealthChanged;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= GameEvents.OnPlayerHealthChanged;
    }
}
