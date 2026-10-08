public readonly struct WeaponStatus
{
    public int Magazine { get; }
    public int MagazineSize { get; }
    public bool UsesMagazine { get; }
    public bool IsReloading { get; }

    public WeaponStatus(Weapon weapon)
    {
        Magazine = weapon.Magazine;
        MagazineSize = weapon.MagazineSize;
        UsesMagazine = weapon.UsesMagazine;
        IsReloading = weapon.IsReloading;
    }
}
