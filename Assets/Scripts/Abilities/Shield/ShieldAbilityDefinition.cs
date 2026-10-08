using UnityEngine;

[CreateAssetMenu(menuName = "Game/Abilities/Force Shield")]
public class ShieldAbilityDefinition : AbilityDefinition
{
    [Header("Shield")]
    public ForceShield shieldPrefab;
    [Tooltip("Seconds the shield stays active")]
    public float duration = 6f;
    public float radius = 2f;
    [Tooltip("Damage the shield can absorb before breaking")]
    public int durability = 150;
    public Vector3 offset = new(0f, 1f, 0f);

    public override IAbility CreateAbility(AbilityContext context)
    {
        return new ShieldAbility(this, context);
    }
}
