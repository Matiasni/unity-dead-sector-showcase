using UnityEngine;

[CreateAssetMenu(menuName = "Game/Shop/Weapon Item")]
public class WeaponShopItem : ShopItem
{
    [Header("Weapon")]
    public WeaponSettings weapon;

    public override PoolableObject DisplayPrefab => weapon.viewPrefab;

    public override bool CanGrant(GameObject buyer, out LocalizedMessage reason)
    {
        reason = LocalizedMessage.Empty;

        if (!buyer.TryGetComponent(out IWeaponHolder holder))
        {
            reason = new LocalizedMessage("Can't carry weapons");
            return false;
        }

        if (holder.HasWeapon(weapon))
        {
            reason = new LocalizedMessage("{0} already equipped", weapon.weaponName);
            return false;
        }

        return true;
    }

    public override void Grant(GameObject buyer)
    {
        buyer.GetComponent<IWeaponHolder>().Equip(weapon);
    }
}
