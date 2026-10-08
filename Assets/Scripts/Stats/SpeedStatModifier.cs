public class SpeedStatModifier : IMovementModifier
{
    private readonly IStatProvider stats;

    public SpeedStatModifier(IStatProvider stats)
    {
        this.stats = stats;
    }

    public float GetSpeedMultiplier() => stats.Apply(StatType.MoveSpeed, 1f);

    public bool IsFinished => false;

    public void Tick(float deltaTime) { }
}
