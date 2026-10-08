using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInputHandler : MonoBehaviour,
    ICharacterInput,
    ICharacterInputEvents,
    IWeaponInput,
    IInteractionInput,
    IAbilityInput
{
    public event Action OnShootStarted;
    public event Action OnShootCanceled;

    public event Action OnAimStarted;
    public event Action OnAimCanceled;

    private PlayerInputActions inputActions;

    private Vector2 move;
    private bool dashPressed;

    private bool shootHeld;
    private bool aimHeld;

    private bool switchWeaponPressed;
    private int selectedSlot = -1;

    private bool interactPressed;
    private int abilitySlotPressed = -1;
    private bool healPressed;
    private bool reloadPressed;

    public Vector2 Move => move;
    public bool DashPressed => dashPressed;

    public bool ShootHeld => shootHeld;
    public bool AimHeld => aimHeld;

    public bool SwitchWeaponPressed => switchWeaponPressed;
    public int SelectedSlot => selectedSlot;

    public bool InteractPressed => interactPressed;
    public int AbilitySlotPressed => abilitySlotPressed;
    public bool HealPressed => healPressed;
    public bool ReloadPressed => reloadPressed;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Dash.started += OnDash;

        inputActions.Player.Shoot.started += OnShoot;
        inputActions.Player.Shoot.canceled += OnShoot;

        inputActions.Player.Aim.started += OnAim;
        inputActions.Player.Aim.canceled += OnAim;

        inputActions.Player.SwitchWeapon.started += OnSwitchWeapon;
        inputActions.Player.WeaponSlot1.started += OnWeaponSlot1;
        inputActions.Player.WeaponSlot2.started += OnWeaponSlot2;

        inputActions.Player.Interact.started += OnInteract;

        inputActions.Player.Ability1.started += OnAbility1;
        inputActions.Player.Ability2.started += OnAbility2;
        inputActions.Player.Heal.started += OnHeal;
        inputActions.Player.Reload.started += OnReload;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Player.Dash.started -= OnDash;

        inputActions.Player.Shoot.started -= OnShoot;
        inputActions.Player.Shoot.canceled -= OnShoot;

        inputActions.Player.Aim.started -= OnAim;
        inputActions.Player.Aim.canceled -= OnAim;

        inputActions.Player.SwitchWeapon.started -= OnSwitchWeapon;
        inputActions.Player.WeaponSlot1.started -= OnWeaponSlot1;
        inputActions.Player.WeaponSlot2.started -= OnWeaponSlot2;

        inputActions.Player.Interact.started -= OnInteract;

        inputActions.Player.Ability1.started -= OnAbility1;
        inputActions.Player.Ability2.started -= OnAbility2;
        inputActions.Player.Heal.started -= OnHeal;
        inputActions.Player.Reload.started -= OnReload;

        inputActions.Player.Disable();

        move = Vector2.zero;
        shootHeld = false;
        aimHeld = false;
    }

    private void OnDestroy()
    {
        inputActions.Dispose();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (context.started)
            dashPressed = true;
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            shootHeld = true;
            OnShootStarted?.Invoke();
        }
        else if (context.canceled)
        {
            shootHeld = false;
            OnShootCanceled?.Invoke();
        }
    }

    private void OnAim(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            aimHeld = true;
            OnAimStarted?.Invoke();
        }
        else if (context.canceled)
        {
            aimHeld = false;
            OnAimCanceled?.Invoke();
        }
    }

    private void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        switchWeaponPressed = true;
    }

    private void OnWeaponSlot1(InputAction.CallbackContext context)
    {
        selectedSlot = 0;
    }

    private void OnWeaponSlot2(InputAction.CallbackContext context)
    {
        selectedSlot = 1;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        interactPressed = true;
    }

    private void OnAbility1(InputAction.CallbackContext context)
    {
        abilitySlotPressed = 0;
    }

    private void OnAbility2(InputAction.CallbackContext context)
    {
        abilitySlotPressed = 1;
    }

    private void OnHeal(InputAction.CallbackContext context)
    {
        healPressed = true;
    }

    private void OnReload(InputAction.CallbackContext context)
    {
        reloadPressed = true;
    }

    private void LateUpdate()
    {
        dashPressed = false;
        switchWeaponPressed = false;
        selectedSlot = -1;
        interactPressed = false;
        abilitySlotPressed = -1;
        healPressed = false;
        reloadPressed = false;
    }
}
