using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UINotificationFeed : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private int maxLines = 5;
    [SerializeField] private float lineDuration = 2.5f;

    private readonly Queue<(LocalizedMessage message, float expireTime)> lines = new();
    private readonly StringBuilder builder = new();

    private void Awake()
    {
        label.text = string.Empty;
    }

    private void OnEnable()
    {
        GameEvents.onNotification += Push;
        GameEvents.onPlayerResourceChanged += PushReward;
    }

    private void OnDestroy()
    {
        GameEvents.onNotification -= Push;
        GameEvents.onPlayerResourceChanged -= PushReward;
    }

    private void Update()
    {
        if (lines.Count == 0 || lines.Peek().expireTime > Time.time) return;

        while (lines.Count > 0 && lines.Peek().expireTime <= Time.time)
            lines.Dequeue();

        Rebuild();
    }

    private void PushReward(ResourceDefinition resource, int amount, int delta)
    {
        if (delta > 0)
            Push(new LocalizedMessage("+{0} {1}", delta, resource.displayName));
    }

    private void Push(LocalizedMessage message)
    {
        lines.Enqueue((message, Time.time + lineDuration));

        while (lines.Count > maxLines)
            lines.Dequeue();

        Rebuild();
    }

    private void Rebuild()
    {
        builder.Clear();

        foreach (var line in lines)
            builder.AppendLine(LocalizationManager.Instance.Resolve(line.message));

        label.text = builder.ToString();
    }
}
