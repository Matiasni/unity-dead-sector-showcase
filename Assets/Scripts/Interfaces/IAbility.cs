public interface IAbility
{
    AbilityDefinition Definition { get; }
    bool IsReady { get; }
    float CooldownNormalized { get; }
    float ActiveProgress { get; }

    bool HasCharges { get; }
    int Charges { get; }
    int MaxCharges { get; }

    bool TryActivate();
    void Tick(float deltaTime);
    void Dispose();
}
