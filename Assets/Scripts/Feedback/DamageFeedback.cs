using UnityEngine;

[RequireComponent(typeof(HealthBehaviour))]
public class DamageFeedback : MonoBehaviour
{
    [SerializeField] private DamageNumber numberPrefab;
    [SerializeField] private Color numberColor = Color.white;
    [SerializeField] private float numberHeight = 2.2f;
    [SerializeField] private float numberScale = 0.18f;
    [SerializeField] private SoundEffect hitSound;
    [SerializeField] private SoundEffect deathSound;

    private HealthBehaviour health;

    private void Awake()
    {
        health = GetComponent<HealthBehaviour>();
    }

    private void OnEnable()
    {
        health.OnDamaged += ShowDamage;
        health.OnDied += PlayDeath;
    }

    private void OnDisable()
    {
        health.OnDamaged -= ShowDamage;
        health.OnDied -= PlayDeath;
    }

    private void ShowDamage(int amount)
    {
        SoundPlayer.Play(hitSound, transform.position);

        if (numberPrefab == null) return;

        var number = PoolManager.Spawn(numberPrefab, transform.position + Vector3.up * numberHeight, Quaternion.identity);
        number.Show(amount, numberColor, numberScale * (amount >= 40 ? 1.5f : 1f));
    }

    private void PlayDeath()
    {
        SoundPlayer.Play(deathSound, transform.position);
    }
}
