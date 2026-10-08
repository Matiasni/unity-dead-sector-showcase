using UnityEngine;

public interface IMovementModifier
{
    float GetSpeedMultiplier();
    bool IsFinished { get; }
    void Tick(float deltaTime);
}