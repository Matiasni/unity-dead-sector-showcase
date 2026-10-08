public readonly struct MissionResult
{
    public string MissionName { get; }
    public bool Success { get; }
    public int EarnedXp { get; }
    public int ExtractionBonusXp { get; }
    public int ScrapXp { get; }
    public int TotalXp => EarnedXp + ExtractionBonusXp + ScrapXp;

    public MissionResult(string missionName, bool success, int earnedXp, int extractionBonusXp, int scrapXp)
    {
        MissionName = missionName;
        Success = success;
        EarnedXp = earnedXp;
        ExtractionBonusXp = extractionBonusXp;
        ScrapXp = scrapXp;
    }

    public static MissionResult From(MissionDefinition definition, IResourceWallet wallet, bool success)
    {
        int earned = wallet.Get(definition.xpResource);
        int bonus = success ? definition.extractionBonusXp : 0;
        int scrap = success ? wallet.Get(definition.scrapResource) * definition.xpPerScrap : 0;

        return new MissionResult(definition.missionName, success, earned, bonus, scrap);
    }
}
