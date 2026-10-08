using System;

public interface IStatProvider
{
    float GetFlat(StatType stat);
    float GetPercent(StatType stat);

    event Action OnStatsChanged;
}
