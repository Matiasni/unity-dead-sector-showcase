using UnityEngine;

public class DisableOnDeath : MonoBehaviour
{
    [SerializeField] private HealthBehaviour health;
    [SerializeField] private Behaviour[] behaviours;

    private void OnEnable()
    {
        health.OnDied += DisableBehaviours;
        health.OnRevived += EnableBehaviours;
    }

    private void OnDisable()
    {
        health.OnDied -= DisableBehaviours;
        health.OnRevived -= EnableBehaviours;
    }

    private void DisableBehaviours()
    {
        SetBehavioursEnabled(false);
    }

    private void EnableBehaviours()
    {
        SetBehavioursEnabled(true);
    }

    private void SetBehavioursEnabled(bool value)
    {
        foreach (var behaviour in behaviours)
            behaviour.enabled = value;
    }
}
