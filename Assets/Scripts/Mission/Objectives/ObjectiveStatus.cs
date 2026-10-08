using UnityEngine;

public readonly struct ObjectiveStatus
{
    public string Label { get; }
    public string Title { get; }
    public LocalizedMessage Progress { get; }
    public ObjectiveState State { get; }
    public Vector3 Position { get; }

    public ObjectiveStatus(string label, string title, LocalizedMessage progress, ObjectiveState state, Vector3 position)
    {
        Label = label;
        Title = title;
        Progress = progress;
        State = state;
        Position = position;
    }
}
