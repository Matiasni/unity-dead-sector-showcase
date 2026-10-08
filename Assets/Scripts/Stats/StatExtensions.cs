using UnityEngine;

public static class StatExtensions
{
    public static float Apply(this IStatProvider stats, StatType stat, float baseValue)
    {
        if (stats == null) return baseValue;

        return (baseValue + stats.GetFlat(stat)) * Mathf.Max(0f, 1f + stats.GetPercent(stat));
    }

    public static int ApplyRounded(this IStatProvider stats, StatType stat, int baseValue)
    {
        return Mathf.RoundToInt(stats.Apply(stat, baseValue));
    }

    public static float Flat(this IStatProvider stats, StatType stat)
    {
        return stats != null ? stats.GetFlat(stat) : 0f;
    }
}
