using UnityEngine;

public class ReloadIndicator : MonoBehaviour
{
    [SerializeField] private WeaponInventory inventory;
    [SerializeField] private GameObject barRoot;
    [SerializeField] private Transform fillPivot;

    private Camera viewCamera;

    private void Awake()
    {
        barRoot.SetActive(false);
    }

    private void LateUpdate()
    {
        var weapon = inventory.ActiveWeapon;
        bool isReloading = weapon != null && weapon.IsReloading;

        if (barRoot.activeSelf != isReloading)
            barRoot.SetActive(isReloading);

        if (!isReloading) return;

        fillPivot.localScale = new Vector3(Mathf.Clamp01(weapon.ReloadProgress), 1f, 1f);

        if (viewCamera == null)
            viewCamera = Camera.main;

        if (viewCamera != null)
            barRoot.transform.rotation = viewCamera.transform.rotation;
    }
}
