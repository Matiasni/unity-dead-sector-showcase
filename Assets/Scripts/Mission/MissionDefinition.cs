using UnityEngine;

[CreateAssetMenu(menuName = "Game/Mission/Mission Definition")]
public class MissionDefinition : ScriptableObject
{
    [Header("General")]
    public string missionName = "Operation Antenna";
    [Min(0)] public int reinforcements = 3;

    [Header("Results")]
    public ResourceDefinition xpResource;
    public ResourceDefinition scrapResource;
    [Tooltip("XP granted for extracting")]
    public int extractionBonusXp = 150;
    [Tooltip("XP granted per leftover scrap when extracting")]
    public int xpPerScrap = 1;
}
