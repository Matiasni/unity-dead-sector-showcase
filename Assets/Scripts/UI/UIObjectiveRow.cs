using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIObjectiveRow : MonoBehaviour
{
    [SerializeField] private Image check;
    [SerializeField] private Image marker;
    [SerializeField] private TMP_Text titleLabel;
    [SerializeField] private TMP_Text progressLabel;

    public void Show(ObjectiveStatus objective, ObjectiveStyle style)
    {
        var localization = LocalizationManager.Instance;

        gameObject.SetActive(true);

        check.sprite = style.GetCheck(objective.State);
        marker.sprite = style.GetMarker(objective);

        string title = localization.Get(objective.Title);

        if (objective.State == ObjectiveState.Locked)
            title = $"{title} {localization.Get("(locked)")}";

        titleLabel.text = title;
        titleLabel.color = style.GetTextColor(objective);

        progressLabel.text = objective.State == ObjectiveState.Active ? localization.Resolve(objective.Progress) : string.Empty;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
