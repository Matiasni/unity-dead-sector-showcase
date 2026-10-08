using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void Load(string sceneName)
    {
        GamePause.Clear();

        if (PersistentSystem.Instance != null && PersistentSystem.Instance.ScreenManager != null)
            PersistentSystem.Instance.ScreenManager.LoadScene(sceneName);
        else
            SceneManager.LoadScene(sceneName);
    }

    public static void ReloadCurrent()
    {
        Load(SceneManager.GetActiveScene().name);
    }
}
