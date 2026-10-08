using UnityEngine;

[CreateAssetMenu(menuName = "Game/Abilities/Dash")]
public class DashAbilityDefinition : AbilityDefinition
{
    [Header("Dash")]
    [Tooltip("Dash speed in units per second")]
    public float speed = 18f;
    [Tooltip("How long the dash lasts")]
    public float duration = 0.18f;
    [Tooltip("How long shooting and aiming stay blocked after dashing")]
    public float combatLockDuration = 0.25f;
    [Tooltip("Seconds the character ignores damage after dashing")]
    public float invulnerabilityDuration = 0.2f;

    public override IAbility CreateAbility(AbilityContext context)
    {
        return new DashAbility(this, context);
    }
}
