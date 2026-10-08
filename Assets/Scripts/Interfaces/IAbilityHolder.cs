public interface IAbilityHolder
{
    bool HasAbility(AbilityDefinition definition);
    void Equip(AbilityDefinition definition);
}
