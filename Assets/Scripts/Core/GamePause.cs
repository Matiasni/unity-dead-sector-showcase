using System;
using System.Collections.Generic;
using UnityEngine;

public static class GamePause
{
    private static readonly HashSet<object> requests = new();

    public static bool IsPaused => requests.Count > 0;

    public static event Action<bool> OnPauseChanged;

    public static void Request(object owner)
    {
        if (requests.Add(owner))
            Refresh();
    }

    public static void Release(object owner)
    {
        if (requests.Remove(owner))
            Refresh();
    }

    public static void Clear()
    {
        requests.Clear();
        Refresh();
    }

    private static void Refresh()
    {
        Time.timeScale = IsPaused ? 0f : 1f;
        OnPauseChanged?.Invoke(IsPaused);
    }
}
