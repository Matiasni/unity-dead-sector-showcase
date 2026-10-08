using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        UIEvents.SetScreen(ScreenType.LoadingScreen, true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            UIEvents.SetLoadingProgress(progress);

            yield return null;
        }

        UIEvents.SetScreen(ScreenType.LoadingScreen, false);
    }
}