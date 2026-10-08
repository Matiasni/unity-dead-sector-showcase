using UnityEngine;

public abstract class UpgradeDefinition : ScriptableObject
{
    [Header("General")]
    public string upgradeName;
    [TextArea] public string description;
    public UpgradeCategory category;
    [Min(1), Tooltip("How many times it can be picked in a mission")]
    public int maxStacks = 1;

    public abstract void Apply(GameObject target);
}
