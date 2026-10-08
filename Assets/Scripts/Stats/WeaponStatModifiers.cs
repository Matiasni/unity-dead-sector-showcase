public class WeaponStatModifiers : IWeaponModifiers
{
    private readonly IStatProvider stats;
    private readonly StatType? damageStat;
    private readonly StatType? fireRateStat;
    private readonly StatType? magazineStat;
    private readonly StatType? reloadStat;

    public WeaponStatModifiers(IStatProvider stats, StatType? damageStat, StatType? fireRateStat, StatType? magazineStat, StatType? reloadStat)
    {
        this.stats = stats;
        this.damageStat = damageStat;
        this.fireRateStat = fireRateStat;
        this.magazineStat = magazineStat;
        this.reloadStat = reloadStat;
    }

    public static WeaponStatModifiers ForPlayer(IStatProvider stats)
    {
        return new WeaponStatModifiers(stats, StatType.WeaponDamage, StatType.WeaponFireRate, StatType.MagazineSize, StatType.ReloadTime);
    }

    public static WeaponStatModifiers ForDrone(IStatProvider stats)
    {
        return new WeaponStatModifiers(stats, null, StatType.DroneFireRate, null, null);
    }

    public int ModifyDamage(int damage) => damageStat.HasValue ? stats.ApplyRounded(damageStat.Value, damage) : damage;

    public float ModifyFireRate(float fireRate) => fireRateStat.HasValue ? stats.Apply(fireRateStat.Value, fireRate) : fireRate;

    public int ModifyMagazineSize(int magazineSize) => magazineStat.HasValue ? stats.ApplyRounded(magazineStat.Value, magazineSize) : magazineSize;

    public float ModifyReloadTime(float reloadTime) => reloadStat.HasValue ? stats.Apply(reloadStat.Value, reloadTime) : reloadTime;
}
