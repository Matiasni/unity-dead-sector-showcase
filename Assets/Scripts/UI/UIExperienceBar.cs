using UnityEngine;
using UnityEngine.UI;

public class UIExperienceBar : MonoBehaviour
{
    [SerializeField] private Slider bar;
    [SerializeField] private Text levelLabel;
    [SerializeField] private Text xpLabel;

    private void OnEnable()
    {
        GameEvents.onExperienceChanged += UpdateExperience;
        GameEvents.onPlayerLevelUp += AnnounceLevelUp;
    }

    private void OnDestroy()
    {
        GameEvents.onExperienceChanged -= UpdateExperience;
        GameEvents.onPlayerLevelUp -= AnnounceLevelUp;
    }

    private void UpdateExperience(ExperienceStatus status)
    {
        bar.value = status.Progress;
        levelLabel.text = LocalizationManager.Instance.Resolve(new LocalizedMessage("LVL {0}", status.Level));
        xpLabel.text = LocalizationManager.Instance.Resolve(new LocalizedMessage("{0} / {1} XP", status.Xp, status.NextLevelXp));
    }

    private void AnnounceLevelUp(int level)
    {
        GameEvents.OnNotification("Level up! LVL {0}", level);
    }
}
