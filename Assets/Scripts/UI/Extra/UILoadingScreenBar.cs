using UnityEngine;
using UnityEngine.UI;

public class UILoadingScreenBar : MonoBehaviour
{
    public Slider progressbar;

    private void OnEnable()
    {
        UIEvents.LoadingProgress += ProgressBar;
    }

    private void OnDisable()
    {
        UIEvents.LoadingProgress -= ProgressBar;
    }

    public void ProgressBar(float progress)
    {
        progressbar.value = progress;
    }
}