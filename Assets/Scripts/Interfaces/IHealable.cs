public interface IHealable
{
    bool CanHeal { get; }
    void Heal(int amount);
}
