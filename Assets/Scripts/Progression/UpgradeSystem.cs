using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LevelProgression))]
public class UpgradeSystem : MonoBehaviour
{
    [SerializeField] private UpgradePool pool;
    [SerializeField, Min(1)] private int optionsPerLevel = 3;

    private readonly Dictionary<UpgradeDefinition, int> stacks = new();

    private LevelProgression progression;
    private UpgradeDefinition[] currentOptions;
    private int pendingChoices;

    public IReadOnlyDictionary<UpgradeDefinition, int> Stacks => stacks;
    public bool IsChoosing => currentOptions != null;

    public event Action<UpgradeDefinition[]> OnUpgradesOffered;

    private void Awake()
    {
        progression = GetComponent<LevelProgression>();
    }

    private void OnEnable()
    {
        progression.OnLevelUp += HandleLevelUp;
    }

    private void OnDisable()
    {
        progression.OnLevelUp -= HandleLevelUp;
        GamePause.Release(this);
    }

    public void Choose(UpgradeDefinition upgrade)
    {
        if (!IsChoosing || !currentOptions.Contains(upgrade)) return;

        currentOptions = null;

        stacks.TryGetValue(upgrade, out int taken);
        stacks[upgrade] = taken + 1;

        upgrade.Apply(gameObject);

        if (pendingChoices > 0)
            OfferNext();
        else
            GamePause.Release(this);
    }

    private void HandleLevelUp(int level)
    {
        pendingChoices++;

        if (!IsChoosing)
            OfferNext();
    }

    private void OfferNext()
    {
        pendingChoices--;

        var options = pool.PickOptions(optionsPerLevel, stacks);

        if (options.Length == 0)
        {
            GamePause.Release(this);
            return;
        }

        currentOptions = options;
        GamePause.Request(this);
        OnUpgradesOffered?.Invoke(options);
    }
}
