using UnityEngine;

[CreateAssetMenu(menuName = "Game/Abilities/Grenade")]
public class GrenadeAbilityDefinition : AbilityDefinition
{
    [Header("Grenade")]
    public GrenadeSettings grenade;

    [Header("Throw")]
    [Tooltip("Max distance from the player to the landing point")]
    public float maxThrowDistance = 14f;
    [Tooltip("Seconds the grenade takes to reach the target point")]
    public float flightTime = 0.8f;

    public override IAbility CreateAbility(AbilityContext context)
    {
        return new GrenadeAbility(this, context);
    }
}
