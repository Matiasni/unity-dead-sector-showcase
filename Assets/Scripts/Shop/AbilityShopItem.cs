using UnityEngine;

[CreateAssetMenu(menuName = "Game/Shop/Ability Item")]
public class AbilityShopItem : ShopItem
{
    [Header("Ability")]
    public AbilityDefinition ability;

    public override bool CanGrant(GameObject buyer, out LocalizedMessage reason)
    {
        reason = LocalizedMessage.Empty;

        if (!buyer.TryGetComponent(out IAbilityHolder holder))
        {
            reason = new LocalizedMessage("Can't use abilities");
            return false;
        }

        if (holder.HasAbility(ability))
        {
            reason = new LocalizedMessage("{0} already equipped", ability.abilityName);
            return false;
        }

        return true;
    }

    public override void Grant(GameObject buyer)
    {
        buyer.GetComponent<IAbilityHolder>().Equip(ability);
    }
}
