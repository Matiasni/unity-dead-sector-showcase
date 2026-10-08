using UnityEngine;

public class SessionLoadoutProvider : MonoBehaviour, ILoadoutProvider
{
    [SerializeField] private LoadoutCatalog catalog;

    public WeaponSettings[] Weapons => GameSession.HasLoadout ? GameSession.Weapons : catalog.defaultWeapons;
    public AbilityDefinition[] Abilities => GameSession.HasLoadout ? GameSession.Abilities : catalog.defaultAbilities;
}
