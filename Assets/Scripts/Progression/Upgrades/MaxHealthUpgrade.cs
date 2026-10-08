using UnityEngine;

[CreateAssetMenu(menuName = "Game/Progression/Upgrades/Max Health Upgrade")]
public class MaxHealthUpgrade : UpgradeDefinition
{
    [Header("Health")]
    [Range(0f, 1f), Tooltip("Percent of the base max health added")]
    public float percent = 0.2f;

    public override void Apply(GameObject target)
    {
        if (target.TryGetComponent(out IMaxHealthReceiver health))
            health.IncreaseMaxHealth(Mathf.RoundToInt(health.MaxHealth * percent));
    }
}
