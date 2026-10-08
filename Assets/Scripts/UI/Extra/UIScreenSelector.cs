using UnityEngine;

public enum ScreenType
{
    Settings,
    LoadingScreen,
    MissionResults,
    Pause,
    UpgradeChoice
}

public class UIScreenSelector : MonoBehaviour
{
    [SerializeField] private ScreenType screenType;

    public void OpenScreen()
    {
        UIEvents.SetScreen(screenType, true);
    }

    public void CloseScreen()
    {
        UIEvents.SetScreen(screenType, false);
    }
}