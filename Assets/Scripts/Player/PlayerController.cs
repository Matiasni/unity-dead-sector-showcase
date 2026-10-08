using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private ICharacterInput input;
    private IWeaponInput weaponInput;
    private IAbilityInput abilityInput;
    private IAimProvider aimProvider;

    private MovementBehaviour movement;
    private RotationBehaviour rotation;
    private CharacterCombatState combat;

    private WeaponInventory inventory;
    private AbilityController abilities;

    private void Awake()
    {
        input = GetComponent<ICharacterInput>();
        weaponInput = GetComponent<IWeaponInput>();
        abilityInput = GetComponent<IAbilityInput>();
        aimProvider = GetComponent<IAimProvider>();
        movement = GetComponent<MovementBehaviour>();
        rotation = GetComponent<RotationBehaviour>();
        combat = GetComponent<CharacterCombatState>();
        inventory = GetComponent<WeaponInventory>();
        abilities = GetComponent<AbilityController>();

        combat.Initialize(
            movement,
            movement.Config.Combat.moveSpeedMultiplier
        );

        var stats = GetComponent<IStatProvider>();

        if (stats != null)
            movement.AddModifier(new SpeedStatModifier(stats));
    }

    private void Update()
    {
        if (input == null || GamePause.IsPaused) return;

        movement.SetMoveInput(input.Move);

        UpdateAbilities();

        combat.SetAiming(input.AimHeld);
        combat.SetShooting(input.ShootHeld);

        UpdateRotation();
        UpdateWeaponSelection();

        inventory.ActiveWeapon.SetTrigger(combat.IsShooting);
    }

    private void UpdateAbilities()
    {
        if (abilityInput == null || abilities == null) return;

        if (abilityInput.DashPressed)
            abilities.TryActivateMovement();

        if (abilityInput.HealPressed)
            abilities.TryActivateHeal();

        if (abilityInput.AbilitySlotPressed >= 0)
            abilities.TryActivate(abilityInput.AbilitySlotPressed);
    }

    private void UpdateRotation()
    {
        Vector3 direction = combat.IsAiming ? GetAimDirection() : movement.MoveDirection;
        rotation.SetLookDirection(direction);
    }

    private void UpdateWeaponSelection()
    {
        if (weaponInput == null) return;

        if (weaponInput.SwitchWeaponPressed)
            inventory.SwitchWeapon();
        else if (weaponInput.SelectedSlot >= 0)
            inventory.SelectSlot(weaponInput.SelectedSlot);

        if (weaponInput.ReloadPressed)
            inventory.TryReloadActive();
    }

    private Vector3 GetAimDirection()
    {
        Vector3 direction = aimProvider.GetAimPoint() - transform.position;
        direction.y = 0f;

        return direction;
    }

    private void OnDisable()
    {
        movement.SetMoveInput(Vector2.zero);
        combat.Interrupt();
        inventory.ActiveWeapon?.ReleaseTrigger();
    }
}
