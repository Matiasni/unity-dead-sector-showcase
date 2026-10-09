using UnityEngine;

public class UILanguageSelector : MonoBehaviour
{
    [SerializeField] private StyledButton[] buttons;
    [SerializeField] private string[] languageCodes;
    [SerializeField] private ButtonStyle activeStyle;
    [SerializeField] private ButtonStyle idleStyle;

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
            buttons[i].SetStyle(languageCodes[i] == current ? activeStyle : idleStyle);
    }
}
