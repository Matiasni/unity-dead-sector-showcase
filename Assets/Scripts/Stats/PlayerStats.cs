using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IStatProvider, IStatReceiver
{
    private readonly Dictionary<StatType, float> flatModifiers = new();
    private readonly Dictionary<StatType, float> percentModifiers = new();

    public event Action OnStatsChanged;

    public float GetFlat(StatType stat) => flatModifiers.TryGetValue(stat, out float value) ? value : 0f;

    public float GetPercent(StatType stat) => percentModifiers.TryGetValue(stat, out float value) ? value : 0f;

    public void AddModifier(StatType stat, StatModifierMode mode, float value)
    {
        var target = mode == StatModifierMode.Flat ? flatModifiers : percentModifiers;

        target.TryGetValue(stat, out float current);
        target[stat] = current + value;

        OnStatsChanged?.Invoke();
    }
}
