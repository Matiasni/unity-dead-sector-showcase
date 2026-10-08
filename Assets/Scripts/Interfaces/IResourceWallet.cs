using System;

public interface IResourceWallet
{
    event Action<ResourceDefinition, int, int> OnResourceChanged;

    int Get(ResourceDefinition resource);
    int GetCapacity(ResourceDefinition resource);
    bool Has(ResourceDefinition resource, int amount);
    bool CanAdd(ResourceDefinition resource);

    int Add(ResourceDefinition resource, int amount);
    bool TrySpend(ResourceDefinition resource, int amount);
    void IncreaseCapacity(ResourceDefinition resource, int amount);
}
