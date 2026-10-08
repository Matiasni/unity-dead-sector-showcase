using UnityEngine;
using UnityEngine.Events;

public class UIScreen : MonoBehaviour
{
    [SerializeField] private ScreenType popupType;
    [SerializeField] private CanvasGroup screenObj;
    [SerializeField] private UnityEvent openEvent;
    [SerializeField] private UnityEvent closeEvent;

    void Awake()
    {
        UIEvents.OnScreenSelection += SetScreen;
    }

    private void OnDestroy()
    {
        UIEvents.OnScreenSelection -= SetScreen;
    }

    private void SetScreen(ScreenType type, bool enable)
    {
        if (type != this.popupType)
            return;

        screenObj?.gameObject.SetActive(enable);
    }

    public void CloseScreen()
    {
        closeEvent?.Invoke();

        screenObj?.gameObject.SetActive(false);
    }

    public void CloseScreenWithoutEvent()
    {
        screenObj?.gameObject.SetActive(false);
    }
}