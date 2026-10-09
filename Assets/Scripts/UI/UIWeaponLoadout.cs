using System.Collections.Generic;
using UnityEngine;

public class UIWeaponLoadout : MonoBehaviour
{
    [SerializeField] private UIWeaponSlot[] slots;
    [SerializeField] private UIPips ammoRow;

    private readonly Dictionary<ResourceDefinition, int> reserves = new();
    private readonly Dictionary<int, WeaponStatus> statuses = new();

    private WeaponSettings[] loadout = new WeaponSettings[0];
    private int activeIndex;
    private bool hasLayout;

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
        for (int i = 0; i < slots.Length; i++)
        {
            var weapon = i < loadout.Length ? loadout[i] : null;
            slots[i].gameObject.SetActive(weapon != null);

            if (weapon == null) continue;

            GetAmmoTexts(i, weapon, out string magazine, out string reserve);
            slots[i].SetWeapon(i, weapon, magazine, reserve, IsOutOfAmmo(i, weapon));
            slots[i].SetSelected(i == activeIndex);
        }

        if (activeIndex < slots.Length)
            slots[activeIndex].transform.SetSiblingIndex(ammoRow.transform.GetSiblingIndex() + 1);

        if (!hasLayout && loadout.Length > 0)
        {
            hasLayout = true;

            foreach (var slot in slots)
                slot.SnapLayout();
        }

        RefreshAmmoRow();
    }

    private void RefreshAmmoRow()
    {
        var weapon = activeIndex < loadout.Length ? loadout[activeIndex] : null;

        if (weapon == null || weapon.ammoIcon == null || !statuses.TryGetValue(activeIndex, out var status) || !status.UsesMagazine)
        {
            ammoRow.Show(0, 0);
            return;
        }

        ammoRow.SetSprites(weapon.ammoIcon, null);
        ammoRow.Show(status.IsReloading ? 0 : status.Magazine, status.MagazineSize);
    }

    private void GetAmmoTexts(int slot, WeaponSettings weapon, out string magazine, out string reserve)
    {
        if (weapon.ammoType == null)
        {
            magazine = "--";
            reserve = string.Empty;
            return;
        }

        int reserveAmount = GetReserve(weapon);

        if (!statuses.TryGetValue(slot, out var status) || !status.UsesMagazine)
        {
            magazine = reserveAmount.ToString();
            reserve = string.Empty;
            return;
        }

        magazine = status.IsReloading ? "--" : status.Magazine.ToString();
        reserve = $"/ {reserveAmount}";
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
