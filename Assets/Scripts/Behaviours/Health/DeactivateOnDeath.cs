using UnityEngine;

[RequireComponent(typeof(HealthBehaviour))]
public class DeactivateOnDeath : MonoBehaviour
{
    private HealthBehaviour health;

    private void Awake()
    {
        health = GetComponent<HealthBehaviour>();
        health.OnDied += Deactivate;
    }

    private void OnDestroy()
    {
        health.OnDied -= Deactivate;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
