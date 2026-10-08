using UnityEngine;

[RequireComponent(typeof(AbilityController))]
public class PlayerAbilitiesBroadcaster : MonoBehaviour
{
    private AbilityController abilities;

    private void Awake()
    {
        abilities = GetComponent<AbilityController>();
    }

    private void OnEnable()
    {
        abilities.OnAbilitiesChanged += GameEvents.OnAbilitiesChanged;
    }

    private void OnDisable()
    {
        abilities.OnAbilitiesChanged -= GameEvents.OnAbilitiesChanged;
    }
}
