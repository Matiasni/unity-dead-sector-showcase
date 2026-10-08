public static class GameSession
{
    public static WeaponSettings[] Weapons { get; private set; }
    public static AbilityDefinition[] Abilities { get; private set; }

    public static bool HasLoadout => Weapons != null && Abilities != null;

    public static MissionResult? LastResult { get; private set; }
    public static int TotalXp { get; private set; }
    public static int MissionsPlayed { get; private set; }

    public static void SetLoadout(WeaponSettings[] weapons, AbilityDefinition[] abilities)
    {
        Weapons = weapons;
        Abilities = abilities;
    }

    public static void RecordResult(MissionResult result)
    {
        LastResult = result;
        TotalXp += result.TotalXp;
        MissionsPlayed++;
    }
}
