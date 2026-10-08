using UnityEngine;
using UnityEngine.UI;

public class UIMissionResults : MonoBehaviour
{
    [SerializeField] private Text titleLabel;
    [SerializeField] private Text breakdownLabel;
    [SerializeField] private Color successColor = new(0.5f, 1f, 0.5f);
    [SerializeField] private Color failureColor = new(1f, 0.4f, 0.35f);

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

        breakdownLabel.text =
            $"{localization.Get(result.MissionName)}\n\n" +
            $"{localization.Get("Earned XP")}   {result.EarnedXp}\n" +
            $"{localization.Get("Extraction bonus")}   {result.ExtractionBonusXp}\n" +
            $"{localization.Get("Leftover scrap")}   {result.ScrapXp}\n\n" +
            $"{localization.Get("TOTAL XP")}   {result.TotalXp}";

        UIEvents.SetScreen(ScreenType.MissionResults, true);
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
