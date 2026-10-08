using UnityEngine;

[CreateAssetMenu(menuName = "Game/Weapons/Weapon Config")]
public class WeaponSettings : ScriptableObject
{
    [Header("General")]
    public string weaponName;
    [TextArea] public string description;
    public WeaponView viewPrefab;

    [Header("Firing")]
    public FireMode fireMode;
    [Tooltip("Shots per second")]
    public float fireRate = 5f;
    public ShotPattern shotPattern;

    [Header("Ammo")]
    [Tooltip("Resource consumed when shooting (empty = infinite ammo)")]
    public ResourceDefinition ammoType;
    [Min(0)] public int ammoPerShot = 1;

    [Header("Magazine")]
    [Min(0), Tooltip("Rounds per magazine (0 = shoots straight from the reserve)")]
    public int magazineSize;
    [Min(0f)] public float reloadTime = 1.5f;

    [Header("Projectile")]
    public Projectile projectilePrefab;
    public float projectileSpeed = 20f;
    public float projectileLifeTime = 2f;
    [Tooltip("Damage per projectile")]
    public int damage = 10;
    [Min(0), Tooltip("Extra targets each projectile goes through")]
    public int pierceCount;

    [Header("Feedback")]
    public SoundEffect fireSound;
    [Range(0f, 1f), Tooltip("Camera trauma added per shot when fired by the player")]
    public float fireShake;
}
