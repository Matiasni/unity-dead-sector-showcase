using UnityEngine;

public class AbilityContext
{
    public GameObject Owner { get; }
    public Transform CastPoint { get; }
    public IAimProvider AimProvider { get; }
    public IResourceWallet Wallet { get; }
    public IStatProvider Stats { get; }

    public AbilityContext(GameObject owner, Transform castPoint, IAimProvider aimProvider, IResourceWallet wallet, IStatProvider stats)
    {
        Owner = owner;
        CastPoint = castPoint;
        AimProvider = aimProvider;
        Wallet = wallet;
        Stats = stats;
    }
}
