using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoadoutScreen : MonoBehaviour
{
    [SerializeField] private LoadoutCatalog catalog;
    [SerializeField] private UILoadoutCard[] weaponCards;
    [SerializeField] private UILoadoutCard[] abilityCards;
    [SerializeField] private Button deployButton;
    [SerializeField] private Text summaryLabel;

    private readonly List<WeaponSettings> selectedWeapons = new();
    private readonly List<AbilityDefinition> selectedAbilities = new();

    private void Start()
    {
        selectedWeapons.AddRange(GameSession.HasLoadout ? GameSession.Weapons : catalog.defaultWeapons);
        selectedAbilities.AddRange(GameSession.HasLoadout ? GameSession.Abilities : catalog.defaultAbilities);

        LocalizationManager.Instance.OnLanguageChanged += RefreshTexts;

        RefreshTexts();
        Refresh();
    }

    private void OnDestroy()
    {
        if (LocalizationManager.HasInstance)
            LocalizationManager.Instance.OnLanguageChanged -= RefreshTexts;
    }

    private void RefreshTexts()
    {
        var localization = LocalizationManager.Instance;

        for (int i = 0; i < weaponCards.Length; i++)
            weaponCards[i].Setup(localization.Get(catalog.weapons[i].weaponName), localization.Get(catalog.weapons[i].description));

        for (int i = 0; i < abilityCards.Length; i++)
            abilityCards[i].Setup(localization.Get(catalog.abilities[i].abilityName), localization.Get(catalog.abilities[i].description));

        summaryLabel.text = BuildSummary();
    }

    public void ToggleWeapon(int index)
    {
        Toggle(selectedWeapons, catalog.weapons[index], WeaponInventory.SlotCount);
        Refresh();
    }

    public void ToggleAbility(int index)
    {
        Toggle(selectedAbilities, catalog.abilities[index], AbilityController.SlotCount);
        Refresh();
    }

    public void Deploy()
    {
        GameSession.SetLoadout(selectedWeapons.ToArray(), selectedAbilities.ToArray());
        SceneLoader.Load(SceneNames.Mission);
    }

    public void BackToMenu()
    {
        SceneLoader.Load(SceneNames.MainMenu);
    }

    private static void Toggle<T>(List<T> selection, T item, int maxCount)
    {
        if (selection.Remove(item)) return;

        if (selection.Count >= maxCount)
            selection.RemoveAt(0);

        selection.Add(item);
    }

    private void Refresh()
    {
        for (int i = 0; i < weaponCards.Length; i++)
            weaponCards[i].SetSelected(selectedWeapons.Contains(catalog.weapons[i]));

        for (int i = 0; i < abilityCards.Length; i++)
            abilityCards[i].SetSelected(selectedAbilities.Contains(catalog.abilities[i]));

        deployButton.interactable = selectedWeapons.Count == WeaponInventory.SlotCount && selectedAbilities.Count == AbilityController.SlotCount;
    }

    private static string BuildSummary()
    {
        var localization = LocalizationManager.Instance;

        if (GameSession.LastResult is not MissionResult last)
            return localization.Get("No missions played yet");

        return localization.Resolve(new LocalizedMessage(
            "Last mission: {0}  -  {1} XP     |     Total XP: {2}  -  Missions: {3}",
            last.Success ? "EXTRACTED" : "FAILED",
            last.TotalXp,
            GameSession.TotalXp,
            GameSession.MissionsPlayed));
    }
}
