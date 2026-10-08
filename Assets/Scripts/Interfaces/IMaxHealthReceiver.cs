public interface IMaxHealthReceiver
{
    int MaxHealth { get; }
    void IncreaseMaxHealth(int amount);
}
