using UnityEngine;

[RequireComponent(typeof(LevelProgression), typeof(UpgradeSystem))]
public class PlayerProgressionBroadcaster : MonoBehaviour
{
    private LevelProgression progression;
    private UpgradeSystem upgrades;

    private void Awake()
    {
        progression = GetComponent<LevelProgression>();
        upgrades = GetComponent<UpgradeSystem>();
    }

    private void OnEnable()
    {
        progression.OnExperienceChanged += GameEvents.OnExperienceChanged;
        progression.OnLevelUp += GameEvents.OnPlayerLevelUp;
        upgrades.OnUpgradesOffered += GameEvents.OnUpgradesOffered;
        GameEvents.onUpgradeSelected += upgrades.Choose;
    }

    private void OnDisable()
    {
        progression.OnExperienceChanged -= GameEvents.OnExperienceChanged;
        progression.OnLevelUp -= GameEvents.OnPlayerLevelUp;
        upgrades.OnUpgradesOffered -= GameEvents.OnUpgradesOffered;
        GameEvents.onUpgradeSelected -= upgrades.Choose;
    }
}
