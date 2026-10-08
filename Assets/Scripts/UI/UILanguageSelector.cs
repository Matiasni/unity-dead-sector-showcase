using UnityEngine;
using UnityEngine.UI;

public class UILanguageSelector : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private string[] languageCodes;
    [SerializeField] private Color selectedColor = new(1f, 0.85f, 0.35f);
    [SerializeField] private Color idleColor = new(0.85f, 0.85f, 0.88f);

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

    public void Select(string languageCode)
    {
        LocalizationManager.Instance.SetLanguage(languageCode);
    }

    private void Refresh()
    {
        string current = LocalizationManager.Instance.CurrentLanguage;

        for (int i = 0; i < buttons.Length; i++)
            buttons[i].GetComponent<Image>().color = languageCodes[i] == current ? selectedColor : idleColor;
    }
}
