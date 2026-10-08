using UnityEngine;

[CreateAssetMenu(menuName = "Game/Abilities/Heal")]
public class HealAbilityDefinition : AbilityDefinition
{
    [Header("Heal")]
    [Min(1), Tooltip("Total health restored")]
    public int healAmount = 60;
    [Min(0f), Tooltip("Seconds to restore the full amount (0 = instant)")]
    public float healDuration = 3f;

    public override IAbility CreateAbility(AbilityContext context)
    {
        return new HealAbility(this, context);
    }
}
