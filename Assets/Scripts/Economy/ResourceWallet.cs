using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceWallet : MonoBehaviour, IResourceWallet
{
    [SerializeField] private ResourceAmount[] startingResources;

    private readonly Dictionary<ResourceDefinition, int> amounts = new();
    private readonly Dictionary<ResourceDefinition, int> bonusCapacity = new();

    public event Action<ResourceDefinition, int, int> OnResourceChanged;

    private void Awake()
    {
        foreach (var entry in startingResources)
        {
            if (entry.resource != null)
                amounts[entry.resource] = Clamp(entry.resource, entry.amount);
        }
    }

    private void Start()
    {
        foreach (var pair in amounts)
            OnResourceChanged?.Invoke(pair.Key, pair.Value, 0);
    }

    public int Get(ResourceDefinition resource)
    {
        return resource != null && amounts.TryGetValue(resource, out int amount) ? amount : 0;
    }

    public int GetCapacity(ResourceDefinition resource)
    {
        if (resource == null || !resource.HasLimit) return int.MaxValue;

        bonusCapacity.TryGetValue(resource, out int bonus);
        return resource.maxAmount + bonus;
    }

    public bool Has(ResourceDefinition resource, int amount)
    {
        return Get(resource) >= amount;
    }

    public bool CanAdd(ResourceDefinition resource)
    {
        return resource != null && Get(resource) < GetCapacity(resource);
    }

    public int Add(ResourceDefinition resource, int amount)
    {
        if (resource == null || amount <= 0) return 0;

        int current = Get(resource);
        int added = Clamp(resource, current + amount) - current;

        if (added <= 0) return 0;

        SetAmount(resource, current + added, added);
        return added;
    }

    public bool TrySpend(ResourceDefinition resource, int amount)
    {
        if (amount <= 0) return true;
        if (!Has(resource, amount)) return false;

        SetAmount(resource, Get(resource) - amount, -amount);
        return true;
    }

    public void IncreaseCapacity(ResourceDefinition resource, int amount)
    {
        if (resource == null || amount <= 0) return;

        bonusCapacity.TryGetValue(resource, out int bonus);
        bonusCapacity[resource] = bonus + amount;
    }

    private void SetAmount(ResourceDefinition resource, int amount, int delta)
    {
        amounts[resource] = amount;
        OnResourceChanged?.Invoke(resource, amount, delta);
    }

    private int Clamp(ResourceDefinition resource, int amount)
    {
        return Mathf.Clamp(amount, 0, GetCapacity(resource));
    }
}
