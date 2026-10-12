using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LevelProgression))]
public class UpgradeSystem : MonoBehaviour
{
    [SerializeField] private UpgradePool pool;
    [SerializeField, Min(1)] private int optionsPerLevel = 3;
    [SerializeField, Min(0f)] private float offerDelay = 0.6f;

    private readonly Dictionary<UpgradeDefinition, int> stacks = new();

    private LevelProgression progression;
    private UpgradeDefinition[] currentOptions;
    private int pendingChoices;
    private float offerTime;
    private bool isOfferScheduled;

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
        isOfferScheduled = false;
        GamePause.Release(this);
    }

    private void Update()
    {
        if (!isOfferScheduled || Time.time < offerTime) return;

        isOfferScheduled = false;
        OfferNext();
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

        if (IsChoosing || isOfferScheduled) return;

        isOfferScheduled = true;
        offerTime = Time.time + offerDelay;
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
