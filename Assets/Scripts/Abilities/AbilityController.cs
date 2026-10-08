using System;
using UnityEngine;

public class AbilityController : MonoBehaviour, IAbilityHolder
{
    public const int SlotCount = 2;

    [SerializeField] private AbilityDefinition movementAbility;
    [SerializeField] private AbilityDefinition healAbility;
    [SerializeField] private AbilityDefinition[] startingAbilities = new AbilityDefinition[SlotCount];
    [SerializeField] private Transform castPoint;

    private readonly IAbility[] slots = new IAbility[SlotCount];
    private IAbility movementSlot;
    private IAbility healSlot;

    private AbilityContext context;

    public event Action<AbilityLoadout> OnAbilitiesChanged;

    private void Awake()
    {
        context = new AbilityContext(
            gameObject,
            castPoint,
            GetComponent<IAimProvider>(),
            GetComponent<IResourceWallet>(),
            GetComponent<IStatProvider>()
        );

        movementSlot = Create(movementAbility);
        healSlot = Create(healAbility);

        var loadout = GetComponent<ILoadoutProvider>()?.Abilities ?? startingAbilities;

        for (int i = 0; i < SlotCount; i++)
            slots[i] = Create(i < loadout.Length && loadout[i] != null ? loadout[i] : startingAbilities[i]);
    }

    private void Start()
    {
        NotifyAbilitiesChanged();
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        movementSlot?.Tick(dt);
        healSlot?.Tick(dt);

        foreach (var ability in slots)
            ability?.Tick(dt);
    }

    public bool TryActivateMovement()
    {
        return movementSlot != null && movementSlot.TryActivate();
    }

    public bool TryActivateHeal()
    {
        return healSlot != null && healSlot.TryActivate();
    }

    public bool TryActivate(int slot)
    {
        if (slot < 0 || slot >= SlotCount) return false;

        return slots[slot] != null && slots[slot].TryActivate();
    }

    public bool HasAbility(AbilityDefinition definition)
    {
        foreach (var ability in slots)
        {
            if (ability != null && ability.Definition == definition)
                return true;
        }

        return false;
    }

    public void Equip(AbilityDefinition definition)
    {
        if (HasAbility(definition)) return;

        slots[SlotCount - 1]?.Dispose();

        for (int i = SlotCount - 1; i > 0; i--)
            slots[i] = slots[i - 1];

        slots[0] = Create(definition);

        NotifyAbilitiesChanged();
    }

    private IAbility Create(AbilityDefinition definition)
    {
        return definition != null ? definition.CreateAbility(context) : null;
    }

    private void NotifyAbilitiesChanged()
    {
        OnAbilitiesChanged?.Invoke(new AbilityLoadout(movementSlot, healSlot, (IAbility[])slots.Clone()));
    }

    private void OnDestroy()
    {
        movementSlot?.Dispose();
        healSlot?.Dispose();

        foreach (var ability in slots)
            ability?.Dispose();
    }
}
