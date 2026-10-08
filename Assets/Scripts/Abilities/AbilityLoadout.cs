public readonly struct AbilityLoadout
{
    public IAbility Movement { get; }
    public IAbility Heal { get; }
    public IAbility[] Slots { get; }

    public AbilityLoadout(IAbility movement, IAbility heal, IAbility[] slots)
    {
        Movement = movement;
        Heal = heal;
        Slots = slots;
    }
}
