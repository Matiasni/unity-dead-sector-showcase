using UnityEngine;

public abstract class AbilityDefinition : ScriptableObject
{
    [Header("General")]
    public string abilityName;
    [TextArea] public string description;
    public Color uiColor = Color.white;
    [Min(0f), Tooltip("Seconds between uses")]
    public float cooldown = 5f;

    [Header("Cost")]
    [Tooltip("Resource consumed on each use (empty = free)")]
    public ResourceDefinition costResource;
    [Min(0)] public int costAmount = 1;

    public bool HasCost => costResource != null && costAmount > 0;

    public abstract IAbility CreateAbility(AbilityContext context);
}
