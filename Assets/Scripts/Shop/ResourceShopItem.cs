using UnityEngine;

[CreateAssetMenu(menuName = "Game/Shop/Resource Item")]
public class ResourceShopItem : ShopItem
{
    [Header("Resource")]
    public ResourceDefinition resource;
    [Min(1)] public int amount = 1;

    public override bool CanGrant(GameObject buyer, out LocalizedMessage reason)
    {
        reason = LocalizedMessage.Empty;

        if (!buyer.TryGetComponent(out IResourceWallet wallet))
        {
            reason = new LocalizedMessage("Can't carry resources");
            return false;
        }

        if (!wallet.CanAdd(resource))
        {
            reason = new LocalizedMessage("{0} is full", resource.displayName);
            return false;
        }

        return true;
    }

    public override void Grant(GameObject buyer)
    {
        buyer.GetComponent<IResourceWallet>().Add(resource, amount);
    }
}
