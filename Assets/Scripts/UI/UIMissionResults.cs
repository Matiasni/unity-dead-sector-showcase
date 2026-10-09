using TMPro;
using UnityEngine;

public class UIMissionResults : MonoBehaviour
{
    [SerializeField] private TMP_Text titleLabel;
    [SerializeField] private TMP_Text missionLabel;
    [SerializeField] private TMP_Text breakdownLabel;
    [SerializeField] private Color successColor = UIPalette.Health;
    [SerializeField] private Color failureColor = UIPalette.Danger;
    [SerializeField] private Color totalColor = UIPalette.Amber;

    private void OnEnable()
    {
        GameEvents.onMissionEnded += ShowResults;
    }

    private void OnDestroy()
    {
        GameEvents.onMissionEnded -= ShowResults;
    }

    private void ShowResults(MissionResult result)
    {
        var localization = LocalizationManager.Instance;

        titleLabel.text = localization.Get(result.Success ? "MISSION COMPLETE" : "MISSION FAILED");
        titleLabel.color = result.Success ? successColor : failureColor;
        missionLabel.text = localization.Get(result.MissionName);

        string total = ColorUtility.ToHtmlStringRGB(totalColor);

        breakdownLabel.text =
            $"{Row(localization.Get("Earned XP"), result.EarnedXp)}\n" +
            $"{Row(localization.Get("Extraction bonus"), result.ExtractionBonusXp)}\n" +
            $"{Row(localization.Get("Leftover scrap"), result.ScrapXp)}\n\n" +
            $"<color=#{total}>{Row(localization.Get("TOTAL XP"), result.TotalXp)}</color>";

        UIEvents.SetScreen(ScreenType.MissionResults, true);
    }

    private static string Row(string label, int value)
    {
        return $"{label}<pos=78%>{value}";
    }

    public void Retry()
    {
        SceneLoader.ReloadCurrent();
    }

    public void ReturnToHub()
    {
        SceneLoader.Load(SceneNames.Hub);
    }
}
