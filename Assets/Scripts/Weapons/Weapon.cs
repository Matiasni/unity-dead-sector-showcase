using System;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    private readonly WeaponSettings settings;
    private readonly WeaponView view;
    private readonly IResourceWallet ammoSource;
    private readonly IWeaponModifiers modifiers;

    private readonly List<Vector3> shotDirections = new();

    private float lastShootTime = -999f;
    private bool wasTriggerHeld;

    private int magazine;
    private float reloadEndTime;

    public WeaponSettings Settings => settings;
    public bool UsesAmmo => settings.ammoType != null && ammoSource != null;
    public bool UsesMagazine => UsesAmmo && settings.magazineSize > 0;

    public int MagazineSize => modifiers != null ? modifiers.ModifyMagazineSize(settings.magazineSize) : settings.magazineSize;
    private int Damage => modifiers != null ? modifiers.ModifyDamage(settings.damage) : settings.damage;
    private float FireRate => modifiers != null ? modifiers.ModifyFireRate(settings.fireRate) : settings.fireRate;
    private float ReloadTime => modifiers != null ? modifiers.ModifyReloadTime(settings.reloadTime) : settings.reloadTime;

    public int Magazine => magazine;
    public int Reserve => UsesAmmo ? ammoSource.Get(settings.ammoType) : 0;
    public bool IsReloading { get; private set; }
    public float ReloadProgress => IsReloading && ReloadTime > 0f ? 1f - (reloadEndTime - Time.time) / ReloadTime : 0f;

    public event Action OnStateChanged;
    public event Action<WeaponSettings> OnFired;

    public Weapon(WeaponSettings settings, WeaponView view, IResourceWallet ammoSource = null, IWeaponModifiers modifiers = null)
    {
        this.settings = settings;
        this.view = view;
        this.ammoSource = ammoSource;
        this.modifiers = modifiers;

        magazine = MagazineSize;
    }

    public void Tick()
    {
        if (IsReloading && Time.time >= reloadEndTime)
            CompleteReload();
    }

    public void SetTrigger(bool held)
    {
        bool pressedThisFrame = held && !wasTriggerHeld;
        wasTriggerHeld = held;

        if (!held) return;

        if (settings.fireMode == FireMode.SemiAutomatic && !pressedThisFrame) return;

        TryShoot();
    }

    public void ReleaseTrigger()
    {
        wasTriggerHeld = false;
    }

    public bool TryReload()
    {
        if (!UsesMagazine || IsReloading) return false;
        if (magazine >= MagazineSize || Reserve <= 0) return false;

        IsReloading = true;
        reloadEndTime = Time.time + ReloadTime;

        OnStateChanged?.Invoke();
        return true;
    }

    public void CancelReload()
    {
        if (!IsReloading) return;

        IsReloading = false;
        OnStateChanged?.Invoke();
    }

    public void SetVisible(bool visible)
    {
        view.gameObject.SetActive(visible);
    }

    public void Dispose()
    {
        view.Release();
    }

    private void TryShoot()
    {
        if (IsReloading) return;

        if (Time.time < lastShootTime + (1f / FireRate))
            return;

        if (!TryConsumeAmmo()) return;

        Shoot();
        lastShootTime = Time.time;

        OnStateChanged?.Invoke();
    }

    private bool TryConsumeAmmo()
    {
        if (!UsesAmmo) return true;

        if (!UsesMagazine)
            return ammoSource.TrySpend(settings.ammoType, settings.ammoPerShot);

        if (magazine < settings.ammoPerShot)
        {
            TryReload();
            return false;
        }

        magazine -= settings.ammoPerShot;
        return true;
    }

    private void CompleteReload()
    {
        IsReloading = false;

        int missing = MagazineSize - magazine;
        int taken = Mathf.Min(missing, Reserve);

        if (taken > 0 && ammoSource.TrySpend(settings.ammoType, taken))
            magazine += taken;

        OnStateChanged?.Invoke();
    }

    private void Shoot()
    {
        Transform muzzle = view.Muzzle;

        SoundPlayer.Play(settings.fireSound, muzzle.position);
        OnFired?.Invoke(settings);

        shotDirections.Clear();
        settings.shotPattern.GetDirections(muzzle.forward, shotDirections);

        foreach (var direction in shotDirections)
        {
            var proj = PoolManager.Spawn(settings.projectilePrefab, muzzle.position, Quaternion.LookRotation(direction));

            proj.Initialize(
                direction,
                settings.projectileSpeed,
                Damage,
                settings.projectileLifeTime,
                settings.pierceCount
            );
        }
    }
}
