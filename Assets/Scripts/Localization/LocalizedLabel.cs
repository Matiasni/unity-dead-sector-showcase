using UnityEngine;

public abstract class LocalizedLabel : MonoBehaviour
{
    [SerializeField] private string key;

    private void Awake()
    {
        if (string.IsNullOrEmpty(key))
            key = ReadText();
    }

    private void OnEnable()
    {
        LocalizationManager.Instance.OnLanguageChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        if (LocalizationManager.HasInstance)
            LocalizationManager.Instance.OnLanguageChanged -= Refresh;
    }

    private void Refresh()
    {
        WriteText(LocalizationManager.Instance.Get(key));
    }

    protected abstract string ReadText();
    protected abstract void WriteText(string value);
}
