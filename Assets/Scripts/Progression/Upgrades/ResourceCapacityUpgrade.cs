using UnityEngine;

[CreateAssetMenu(menuName = "Game/Progression/Upgrades/Resource Capacity Upgrade")]
public class ResourceCapacityUpgrade : UpgradeDefinition
{
    [Header("Resource")]
    public ResourceDefinition resource;
    [Min(1)] public int amount = 1;

    public override void Apply(GameObject target)
    {
        if (!target.TryGetComponent(out IResourceWallet wallet)) return;

        wallet.IncreaseCapacity(resource, amount);
        wallet.Add(resource, amount);
    }
}
