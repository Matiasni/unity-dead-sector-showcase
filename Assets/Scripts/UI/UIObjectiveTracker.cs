using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIObjectiveTracker : MonoBehaviour
{
    [SerializeField] private Text label;
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color completedColor = new(0.5f, 1f, 0.5f, 0.6f);
    [SerializeField] private Color lockedColor = new(1f, 1f, 1f, 0.35f);

    private readonly StringBuilder builder = new();

    private void Awake()
    {
        label.supportRichText = true;
        label.text = string.Empty;
    }

    private void OnEnable()
    {
        GameEvents.onObjectivesChanged += UpdateObjectives;
    }

    private void OnDestroy()
    {
        GameEvents.onObjectivesChanged -= UpdateObjectives;
    }

    private void UpdateObjectives(ObjectiveStatus[] objectives)
    {
        builder.Clear();

        foreach (var objective in objectives)
            builder.AppendLine(FormatLine(objective));

        label.text = builder.ToString();
    }

    private string FormatLine(ObjectiveStatus objective)
    {
        switch (objective.State)
        {
            case ObjectiveState.Completed:
                return Colorize($"[x] {objective.Label} · {Localize(objective.Title)}", completedColor);
            case ObjectiveState.Locked:
                return Colorize($"[ ] {objective.Label} · {Localize(objective.Title)} {Localize("(locked)")}", lockedColor);
            default:
                return Colorize($"[ ] {objective.Label} · {Localize(objective.Title)}  {LocalizationManager.Instance.Resolve(objective.Progress)}", activeColor);
        }
    }

    private static string Localize(string key) => LocalizationManager.Instance.Get(key);

    private static string Colorize(string text, Color color)
    {
        return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{text}</color>";
    }
}
