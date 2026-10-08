using UnityEngine;

public class UIAbilityBar : MonoBehaviour
{
    [SerializeField] private UIAbilitySlot movementSlot;
    [SerializeField] private UIAbilitySlot healSlot;
    [SerializeField] private UIAbilitySlot[] slots;

    private void OnEnable()
    {
        GameEvents.onAbilitiesChanged += UpdateAbilities;
    }

    private void OnDestroy()
    {
        GameEvents.onAbilitiesChanged -= UpdateAbilities;
    }

    private void UpdateAbilities(AbilityLoadout loadout)
    {
        movementSlot.Bind(loadout.Movement);
        healSlot.Bind(loadout.Heal);

        for (int i = 0; i < slots.Length; i++)
            slots[i].Bind(i < loadout.Slots.Length ? loadout.Slots[i] : null);
    }
}
