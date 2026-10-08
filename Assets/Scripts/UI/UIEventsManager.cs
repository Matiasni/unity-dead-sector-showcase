using System;

public static class UIEvents
{
    public static Action<ScreenType, bool> OnScreenSelection;

    public static Action<float> LoadingProgress;

    public static void SetScreen(ScreenType ScreenType, bool enable)
    {
        OnScreenSelection?.Invoke(ScreenType, enable);
    }

    public static void SetLoadingProgress(float loadingProgress)
    {
        LoadingProgress?.Invoke(loadingProgress);
    }
}