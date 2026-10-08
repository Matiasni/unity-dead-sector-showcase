using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Economy/Reward")]
public class RewardDefinition : ScriptableObject
{
    [Serializable]
    public struct RewardEntry
    {
        public ResourceDefinition resource;
        [Min(0)] public int minAmount;
        [Min(0)] public int maxAmount;
        [Range(0f, 1f), Tooltip("Chance of this entry being granted")]
        public float chance;
    }

    [SerializeField] private RewardEntry[] entries;

    public List<ResourceAmount> Roll()
    {
        var results = new List<ResourceAmount>();

        foreach (var entry in entries)
        {
            if (entry.resource == null || UnityEngine.Random.value > entry.chance) continue;

            int amount = UnityEngine.Random.Range(entry.minAmount, Mathf.Max(entry.minAmount, entry.maxAmount) + 1);

            if (amount > 0)
                results.Add(new ResourceAmount(entry.resource, amount));
        }

        return results;
    }

    public bool GrantTo(IResourceWallet wallet)
    {
        bool grantedAny = false;

        foreach (var reward in Roll())
            grantedAny |= wallet.Add(reward.resource, reward.amount) > 0;

        return grantedAny;
    }
}
