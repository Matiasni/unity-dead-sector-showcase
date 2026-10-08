using UnityEngine;

[CreateAssetMenu(menuName = "Game/Loadout/Loadout Catalog")]
public class LoadoutCatalog : ScriptableObject
{
    public WeaponSettings[] weapons;
    public AbilityDefinition[] abilities;

    [Header("Defaults")]
    public WeaponSettings[] defaultWeapons = new WeaponSettings[WeaponInventory.SlotCount];
    public AbilityDefinition[] defaultAbilities = new AbilityDefinition[AbilityController.SlotCount];
}
