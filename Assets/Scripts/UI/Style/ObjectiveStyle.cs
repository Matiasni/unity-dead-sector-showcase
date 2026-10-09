using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/UI/Objective Style")]
public class ObjectiveStyle : ScriptableObject
{
    [Serializable]
    public struct LabelMarker
    {
        public string label;
        public Sprite marker;
        public Color textColor;
    }

    [Header("Markers")]
    public LabelMarker[] markers;
    public Sprite blankMarker;
    public Sprite completedMarker;
    public Sprite lockedMarker;

    [Header("Checks")]
    public Sprite checkEmpty;
    public Sprite checkDone;
    public Sprite checkLocked;

    [Header("Text")]
    public Color activeColor = Color.white;
    public Color completedColor = Color.gray;
    public Color lockedColor = Color.gray;

    public Sprite GetMarker(ObjectiveStatus objective)
    {
        switch (objective.State)
        {
            case ObjectiveState.Completed:
                return completedMarker;
            case ObjectiveState.Locked:
                return lockedMarker;
            default:
                return TryGetLabel(objective.Label, out var entry) ? entry.marker : blankMarker;
        }
    }

    public Sprite GetCheck(ObjectiveState state)
    {
        switch (state)
        {
            case ObjectiveState.Completed:
                return checkDone;
            case ObjectiveState.Locked:
                return checkLocked;
            default:
                return checkEmpty;
        }
    }

    public Color GetTextColor(ObjectiveStatus objective)
    {
        switch (objective.State)
        {
            case ObjectiveState.Completed:
                return completedColor;
            case ObjectiveState.Locked:
                return lockedColor;
            default:
                return TryGetLabel(objective.Label, out var entry) ? entry.textColor : activeColor;
        }
    }

    private bool TryGetLabel(string label, out LabelMarker entry)
    {
        foreach (var marker in markers)
        {
            if (marker.label != label) continue;

            entry = marker;
            return true;
        }

        entry = default;
        return false;
    }
}
