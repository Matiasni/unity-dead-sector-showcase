using UnityEngine;
using UnityEngine.InputSystem;

public class UIPauseMenu : MonoBehaviour
{
    [SerializeField] private InputActionReference pauseAction;

    private bool isOpen;

    private void OnEnable()
    {
        pauseAction.action.Enable();
        pauseAction.action.performed += Toggle;
    }

    private void OnDisable()
    {
        pauseAction.action.performed -= Toggle;
    }

    private void OnDestroy()
    {
        GamePause.Release(this);
    }

    private void Toggle(InputAction.CallbackContext context)
    {
        if (isOpen)
            Resume();
        else if (!GamePause.IsPaused)
            Open();
    }

    private void Open()
    {
        isOpen = true;
        GamePause.Request(this);
        UIEvents.SetScreen(ScreenType.Pause, true);
    }

    public void Resume()
    {
        isOpen = false;
        UIEvents.SetScreen(ScreenType.Pause, false);
        GamePause.Release(this);
    }

    public void Restart()
    {
        SceneLoader.ReloadCurrent();
    }

    public void ReturnToHub()
    {
        SceneLoader.Load(SceneNames.Hub);
    }

    public void QuitToMenu()
    {
        SceneLoader.Load(SceneNames.MainMenu);
    }
}
