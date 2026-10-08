using UnityEngine;

public class CombatModifier : IMovementModifier
{
    private float multiplier;

    public CombatModifier(float multiplier)
    {
        this.multiplier = multiplier;
    }

    public float GetSpeedMultiplier() => multiplier;

    public bool IsFinished => false;

    public void Tick(float deltaTime) { }
}