using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehaviour : MonoBehaviour, IDamageable, IHealable, IDamageModifierHost, ILivingTarget, IMaxHealthReceiver
{
    [SerializeField] private HealthConfig config;

    private readonly List<IDamageModifier> damageModifiers = new();

    private int currentHealth;
    private int bonusMaxHealth;
    private bool isDead;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => config.maxHealth + bonusMaxHealth;
    public bool IsDead => isDead;
    public bool IsAlive => !isDead;
    public bool CanHeal => !isDead && currentHealth < MaxHealth;

    public event Action<int, int> OnHealthChanged;
    public event Action<int> OnDamaged;
    public event Action<int> OnHealed;
    public event Action OnDied;
    public event Action OnRevived;

    private void Awake()
    {
        currentHealth = config.maxHealth;
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(currentHealth, MaxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isDead || amount <= 0) return;

        amount = ApplyDamageModifiers(amount);

        if (amount <= 0) return;

        SetHealth(currentHealth - amount);
        OnDamaged?.Invoke(amount);

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        if (!CanHeal || amount <= 0) return;

        SetHealth(currentHealth + amount);
        OnHealed?.Invoke(amount);
    }

    public void IncreaseMaxHealth(int amount)
    {
        if (amount <= 0) return;

        bonusMaxHealth += amount;
        SetHealth(currentHealth + amount);
    }

    public void AddDamageModifier(IDamageModifier modifier)
    {
        if (!damageModifiers.Contains(modifier))
            damageModifiers.Add(modifier);
    }

    public void RemoveDamageModifier(IDamageModifier modifier)
    {
        damageModifiers.Remove(modifier);
    }

    private int ApplyDamageModifiers(int amount)
    {
        for (int i = damageModifiers.Count - 1; i >= 0 && amount > 0; i--)
        {
            if (i < damageModifiers.Count)
                amount = damageModifiers[i].ModifyDamage(amount);
        }

        return amount;
    }

    public void ResetHealth()
    {
        bool wasDead = isDead;

        isDead = false;
        SetHealth(MaxHealth);

        if (wasDead)
            OnRevived?.Invoke();
    }

    private void SetHealth(int value)
    {
        currentHealth = Mathf.Clamp(value, 0, MaxHealth);
        OnHealthChanged?.Invoke(currentHealth, MaxHealth);
    }

    private void Die()
    {
        isDead = true;
        OnDied?.Invoke();
    }
}
