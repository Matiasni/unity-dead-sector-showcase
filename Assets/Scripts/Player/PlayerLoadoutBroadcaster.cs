using UnityEngine;

[RequireComponent(typeof(WeaponInventory))]
public class PlayerLoadoutBroadcaster : MonoBehaviour
{
    private WeaponInventory inventory;

    private void Awake()
    {
        inventory = GetComponent<WeaponInventory>();
    }

    private void OnEnable()
    {
        inventory.OnLoadoutChanged += GameEvents.OnLoadoutChanged;
        inventory.OnWeaponStatusChanged += GameEvents.OnWeaponStatusChanged;
    }

    private void OnDisable()
    {
        inventory.OnLoadoutChanged -= GameEvents.OnLoadoutChanged;
        inventory.OnWeaponStatusChanged -= GameEvents.OnWeaponStatusChanged;
    }
}
