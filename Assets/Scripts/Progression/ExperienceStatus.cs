public readonly struct ExperienceStatus
{
    public int Xp { get; }
    public int Level { get; }
    public int CurrentLevelXp { get; }
    public int NextLevelXp { get; }

    public float Progress => NextLevelXp <= CurrentLevelXp ? 1f : (float)(Xp - CurrentLevelXp) / (NextLevelXp - CurrentLevelXp);

    public ExperienceStatus(int xp, int level, int currentLevelXp, int nextLevelXp)
    {
        Xp = xp;
        Level = level;
        CurrentLevelXp = currentLevelXp;
        NextLevelXp = nextLevelXp;
    }
}
