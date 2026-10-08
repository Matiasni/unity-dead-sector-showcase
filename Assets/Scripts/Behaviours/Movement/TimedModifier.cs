using UnityEngine;

public abstract class TimedModifier : IMovementModifier
{
    protected float duration;
    protected float elapsed;

    public bool IsFinished => elapsed >= duration;

    public TimedModifier(float duration)
    {
        this.duration = duration;
    }

    public void Tick(float deltaTime)
    {
        elapsed += deltaTime;
    }

    public abstract float GetSpeedMultiplier();
}