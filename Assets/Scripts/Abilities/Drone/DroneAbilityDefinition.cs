using UnityEngine;

[CreateAssetMenu(menuName = "Game/Abilities/Drone")]
public class DroneAbilityDefinition : AbilityDefinition
{
    [Header("Drone")]
    public Drone dronePrefab;
    [Tooltip("Seconds the drone stays active")]
    public float lifeTime = 12f;

    [Header("Orbit")]
    public float orbitRadius = 2f;
    [Tooltip("Degrees per second")]
    public float orbitSpeed = 90f;
    public float orbitHeight = 2.2f;

    [Header("Combat")]
    public WeaponSettings weapon;
    public float detectionRadius = 12f;
    public LayerMask targetMask;

    public override IAbility CreateAbility(AbilityContext context)
    {
        return new DroneAbility(this, context);
    }
}
