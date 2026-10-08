using UnityEngine;

public class EnemyContext
{
    public Transform Self { get; }
    public IEnemyMotor Motor { get; }
    public WeaponView WeaponView { get; }
    public ITelegraph Telegraph { get; }
    public IAimIndicator AimIndicator { get; }
    public IDamageModifierHost DamageHost { get; }

    public EnemyContext(Transform self, IEnemyMotor motor, WeaponView weaponView, ITelegraph telegraph, IAimIndicator aimIndicator, IDamageModifierHost damageHost)
    {
        Self = self;
        Motor = motor;
        WeaponView = weaponView;
        Telegraph = telegraph;
        AimIndicator = aimIndicator;
        DamageHost = damageHost;
    }
}
