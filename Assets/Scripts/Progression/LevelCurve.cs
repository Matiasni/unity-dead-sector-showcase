using UnityEngine;

[CreateAssetMenu(menuName = "Game/Progression/Level Curve")]
public class LevelCurve : ScriptableObject
{
    [Min(1), Tooltip("XP needed to go from level 1 to level 2")]
    [SerializeField] private int firstLevelXp = 80;
    [Min(0f), Tooltip("Extra XP each level costs compared to the previous one (0.2 = +20%)")]
    [SerializeField] private float growthPerLevel = 0.2f;

    public int GetLevel(int xp)
    {
        int level = 1;

        while (xp >= GetThreshold(level + 1))
            level++;

        return level;
    }

    public int GetThreshold(int level)
    {
        float total = 0f;
        float cost = firstLevelXp;

        for (int i = 2; i <= level; i++)
        {
            total += cost;
            cost *= 1f + growthPerLevel;
        }

        return Mathf.RoundToInt(total);
    }
}
