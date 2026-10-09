using UnityEngine;

public class UIObjectiveTracker : MonoBehaviour
{
    [SerializeField] private ObjectiveStyle style;
    [SerializeField] private UIObjectiveRow[] rows;

    private void Awake()
    {
        foreach (var row in rows)
            row.Hide();
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
        for (int i = 0; i < rows.Length; i++)
        {
            if (i < objectives.Length)
                rows[i].Show(objectives[i], style);
            else
                rows[i].Hide();
        }
    }
}
