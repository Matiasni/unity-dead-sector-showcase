using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneLoader.Load(SceneNames.Hub);
    }

    public void OpenSandbox()
    {
        SceneLoader.Load(SceneNames.Sandbox);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
