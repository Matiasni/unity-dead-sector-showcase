using UnityEngine;
using UnityEngine.UI;

public class ReloadIndicator : MonoBehaviour
{
    [SerializeField] private WeaponInventory inventory;
    [SerializeField] private GameObject root;
    [SerializeField] private Image fill;

    private void Awake()
    {
        root.SetActive(false);
    }

    private void LateUpdate()
    {
        var weapon = inventory.ActiveWeapon;
        bool isReloading = weapon != null && weapon.IsReloading;

        if (root.activeSelf != isReloading)
            root.SetActive(isReloading);

        if (isReloading)
            fill.fillAmount = Mathf.Clamp01(weapon.ReloadProgress);
    }
}
