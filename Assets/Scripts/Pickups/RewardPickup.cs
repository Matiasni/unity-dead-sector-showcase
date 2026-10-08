using UnityEngine;

public class RewardPickup : Pickup
{
    [SerializeField] private RewardDefinition reward;

    protected override bool TryApply(Collider other)
    {
        var wallet = other.GetComponentInParent<IResourceWallet>();

        return wallet != null && reward.GrantTo(wallet);
    }
}
