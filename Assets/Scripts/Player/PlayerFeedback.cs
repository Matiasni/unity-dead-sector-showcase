using UnityEngine;

[RequireComponent(typeof(HealthBehaviour), typeof(WeaponInventory))]
public class PlayerFeedback : MonoBehaviour
{
    [SerializeField] private float damageShakePerPoint = 0.012f;
    [SerializeField] private float maxDamageShake = 0.6f;
    [SerializeField] private SoundEffect hurtSound;

    private HealthBehaviour health;
    private WeaponInventory inventory;

    private void Awake()
    {
        health = GetComponent<HealthBehaviour>();
        inventory = GetComponent<WeaponInventory>();
    }

    private void OnEnable()
    {
        health.OnDamaged += HandleDamaged;
        inventory.OnWeaponFired += HandleWeaponFired;
    }

    private void OnDisable()
    {
        health.OnDamaged -= HandleDamaged;
        inventory.OnWeaponFired -= HandleWeaponFired;
    }

    private void HandleDamaged(int amount)
    {
        SoundPlayer.Play(hurtSound, transform.position);
        GameEvents.OnCameraShake(Mathf.Min(maxDamageShake, amount * damageShakePerPoint));
    }

    private void HandleWeaponFired(WeaponSettings weapon)
    {
        if (weapon.fireShake > 0f)
            GameEvents.OnCameraShake(weapon.fireShake);
    }
}
