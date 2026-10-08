using UnityEngine;

[CreateAssetMenu(menuName = "Game/Progression/Upgrades/Stat Upgrade")]
public class StatUpgrade : UpgradeDefinition
{
    [Header("Stat")]
    public StatType stat;
    public StatModifierMode mode = StatModifierMode.Percent;
    [Tooltip("Percent uses 0.15 for +15%")]
    public float value = 0.15f;

    public override void Apply(GameObject target)
    {
        if (target.TryGetComponent(out IStatReceiver stats))
            stats.AddModifier(stat, mode, value);
    }
}
