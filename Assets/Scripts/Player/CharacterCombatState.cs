using UnityEngine;

public class CharacterCombatState : MonoBehaviour, ICombatInterruptible
{
    private MovementBehaviour movement;

    private IMovementModifier combatModifier;

    private bool aimRequested;
    private bool shootRequested;
    private float lockedUntil;

    public bool IsLocked => Time.time < lockedUntil;
    public bool IsAiming => !IsLocked && (aimRequested || shootRequested);
    public bool IsShooting => !IsLocked && shootRequested;

    public void Initialize(MovementBehaviour movement, float combatMultiplier)
    {
        this.movement = movement;

        combatModifier = new CombatModifier(combatMultiplier);
    }

    public void SetShooting(bool value)
    {
        shootRequested = value;
        UpdateCombatState();
    }

    public void SetAiming(bool value)
    {
        aimRequested = value;
        UpdateCombatState();
    }

    public void Interrupt()
    {
        Interrupt(0f);
    }

    public void Interrupt(float duration)
    {
        aimRequested = false;
        shootRequested = false;
        lockedUntil = Time.time + duration;

        movement.RemoveModifier(combatModifier);
    }

    private void UpdateCombatState()
    {
        if (IsAiming)
            movement.AddModifier(combatModifier);
        else
            movement.RemoveModifier(combatModifier);
    }
}
