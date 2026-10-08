using UnityEngine;

[RequireComponent(typeof(HealthBehaviour))]
public class ExperienceOnDeath : MonoBehaviour
{
    [SerializeField, Min(0)] private int experience = 5;

    private HealthBehaviour health;

    private void Awake()
    {
        health = GetComponent<HealthBehaviour>();
    }

    private void OnEnable()
    {
        health.OnDied += GrantExperience;
    }

    private void OnDisable()
    {
        health.OnDied -= GrantExperience;
    }

    private void GrantExperience()
    {
        GameEvents.OnEnemyKilled(experience);
    }
}
