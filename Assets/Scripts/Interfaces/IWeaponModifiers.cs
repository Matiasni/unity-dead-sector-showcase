public interface IWeaponModifiers
{
    int ModifyDamage(int damage);
    float ModifyFireRate(float fireRate);
    int ModifyMagazineSize(int magazineSize);
    float ModifyReloadTime(float reloadTime);
}
