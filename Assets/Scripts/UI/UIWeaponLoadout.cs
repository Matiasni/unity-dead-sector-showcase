using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponLoadout : MonoBehaviour
{
    [SerializeField] private Text[] slotLabels;
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color inactiveColor = new(1f, 1f, 1f, 0.4f);
    [SerializeField] private Color emptyAmmoColor = new(1f, 0.3f, 0.3f);

    private readonly Dictionary<ResourceDefinition, int> reserves = new();
    private readonly Dictionary<int, WeaponStatus> statuses = new();

    private WeaponSettings[] loadout = new WeaponSettings[0];
    private int activeIndex;

    private void OnEnable()
    {
        GameEvents.onLoadoutChanged += UpdateLoadout;
        GameEvents.onPlayerResourceChanged += UpdateReserve;
        GameEvents.onWeaponStatusChanged += UpdateStatus;
    }

    private void OnDestroy()
    {
        GameEvents.onLoadoutChanged -= UpdateLoadout;
        GameEvents.onPlayerResourceChanged -= UpdateReserve;
        GameEvents.onWeaponStatusChanged -= UpdateStatus;
    }

    private void UpdateLoadout(WeaponSettings[] loadout, int activeIndex)
    {
        this.loadout = loadout;
        this.activeIndex = activeIndex;
        Refresh();
    }

    private void UpdateReserve(ResourceDefinition resource, int amount, int delta)
    {
        reserves[resource] = amount;
        Refresh();
    }

    private void UpdateStatus(int slot, WeaponStatus status)
    {
        statuses[slot] = status;
        Refresh();
    }

    private void Refresh()
    {
        for (int i = 0; i < slotLabels.Length; i++)
        {
            var weapon = i < loadout.Length ? loadout[i] : null;

            if (weapon == null)
            {
                slotLabels[i].text = $"{i + 1}  -";
                slotLabels[i].color = inactiveColor;
                continue;
            }

            slotLabels[i].text = $"{i + 1}  {LocalizationManager.Instance.Get(weapon.weaponName)}   {GetAmmoText(i, weapon)}";
            slotLabels[i].color = IsOutOfAmmo(i, weapon)
                ? emptyAmmoColor
                : i == activeIndex ? activeColor : inactiveColor;
        }
    }

    private string GetAmmoText(int slot, WeaponSettings weapon)
    {
        if (weapon.ammoType == null) return string.Empty;

        int reserve = GetReserve(weapon);

        if (!statuses.TryGetValue(slot, out var status) || !status.UsesMagazine)
            return reserve.ToString();

        return status.IsReloading
            ? $"{LocalizationManager.Instance.Get("Reloading...")} / {reserve}"
            : $"{status.Magazine}/{status.MagazineSize}  ({reserve})";
    }

    private bool IsOutOfAmmo(int slot, WeaponSettings weapon)
    {
        if (weapon.ammoType == null) return false;

        int magazine = statuses.TryGetValue(slot, out var status) && status.UsesMagazine ? status.Magazine : 0;
        return magazine < weapon.ammoPerShot && GetReserve(weapon) < weapon.ammoPerShot;
    }

    private int GetReserve(WeaponSettings weapon)
    {
        return reserves.TryGetValue(weapon.ammoType, out int amount) ? amount : 0;
    }
}
