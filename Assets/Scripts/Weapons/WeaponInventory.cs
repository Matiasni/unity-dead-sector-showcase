using System;
using UnityEngine;

public class WeaponInventory : MonoBehaviour, IWeaponHolder
{
    public const int SlotCount = 2;

    [SerializeField] private WeaponSettings[] startingWeapons = new WeaponSettings[SlotCount];
    [SerializeField] private Transform weaponSocket;

    private readonly Weapon[] slots = new Weapon[SlotCount];
    private IResourceWallet ammoSource;
    private IWeaponModifiers modifiers;
    private IStatProvider stats;
    private int activeIndex;

    public Weapon ActiveWeapon => slots[activeIndex];

    public event Action<WeaponSettings[], int> OnLoadoutChanged;
    public event Action<int, WeaponStatus> OnWeaponStatusChanged;
    public event Action<WeaponSettings> OnWeaponFired;

    private void Awake()
    {
        ammoSource = GetComponent<IResourceWallet>();
        stats = GetComponent<IStatProvider>();
        modifiers = stats != null ? WeaponStatModifiers.ForPlayer(stats) : null;

        var loadout = GetComponent<ILoadoutProvider>()?.Weapons ?? startingWeapons;

        for (int i = 0; i < SlotCount; i++)
            SetSlot(i, i < loadout.Length && loadout[i] != null ? loadout[i] : startingWeapons[i]);

        RefreshVisibility();
    }

    private void Start()
    {
        NotifyLoadoutChanged();

        for (int i = 0; i < SlotCount; i++)
            NotifyWeaponStatus(i);
    }

    private void OnEnable()
    {
        if (stats != null)
            stats.OnStatsChanged += NotifyAllWeaponStatus;
    }

    private void OnDisable()
    {
        if (stats != null)
            stats.OnStatsChanged -= NotifyAllWeaponStatus;
    }

    private void NotifyAllWeaponStatus()
    {
        for (int i = 0; i < SlotCount; i++)
            NotifyWeaponStatus(i);
    }

    private void Update()
    {
        ActiveWeapon.Tick();
    }

    public bool TryReloadActive()
    {
        return ActiveWeapon.TryReload();
    }

    public void SelectSlot(int index)
    {
        if (index < 0 || index >= SlotCount || index == activeIndex) return;

        ActiveWeapon.ReleaseTrigger();
        ActiveWeapon.CancelReload();
        activeIndex = index;

        RefreshVisibility();
        NotifyLoadoutChanged();
    }

    public void SwitchWeapon()
    {
        SelectSlot((activeIndex + 1) % SlotCount);
    }

    public void Equip(WeaponSettings settings)
    {
        int equippedIndex = IndexOf(settings);

        if (equippedIndex >= 0)
        {
            SelectSlot(equippedIndex);
            return;
        }

        SetSlot(activeIndex, settings);

        RefreshVisibility();
        NotifyLoadoutChanged();
    }

    private void SetSlot(int index, WeaponSettings settings)
    {
        slots[index]?.Dispose();

        var view = PoolManager.Spawn(settings.viewPrefab, weaponSocket);
        var weapon = new Weapon(settings, view, ammoSource, modifiers);
        weapon.OnStateChanged += () => NotifyWeaponStatus(index);
        weapon.OnFired += fired => OnWeaponFired?.Invoke(fired);

        slots[index] = weapon;
        NotifyWeaponStatus(index);
    }

    private void NotifyWeaponStatus(int index)
    {
        OnWeaponStatusChanged?.Invoke(index, new WeaponStatus(slots[index]));
    }

    public bool HasWeapon(WeaponSettings settings)
    {
        return IndexOf(settings) >= 0;
    }

    private int IndexOf(WeaponSettings settings)
    {
        for (int i = 0; i < SlotCount; i++)
        {
            if (slots[i].Settings == settings)
                return i;
        }

        return -1;
    }

    private void RefreshVisibility()
    {
        for (int i = 0; i < SlotCount; i++)
            slots[i].SetVisible(i == activeIndex);
    }

    private void NotifyLoadoutChanged()
    {
        var loadout = new WeaponSettings[SlotCount];

        for (int i = 0; i < SlotCount; i++)
            loadout[i] = slots[i].Settings;

        OnLoadoutChanged?.Invoke(loadout, activeIndex);
    }
}
