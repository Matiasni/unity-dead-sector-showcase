using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Progression/Upgrade Pool")]
public class UpgradePool : ScriptableObject
{
    [SerializeField] private UpgradeDefinition[] upgrades;

    public UpgradeDefinition[] PickOptions(int count, IReadOnlyDictionary<UpgradeDefinition, int> stacks)
    {
        return upgrades
            .Where(upgrade => upgrade != null && (!stacks.TryGetValue(upgrade, out int taken) || taken < upgrade.maxStacks))
            .OrderBy(_ => Random.value)
            .Take(count)
            .ToArray();
    }
}
