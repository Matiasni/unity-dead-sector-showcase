using TMPro;
using UnityEngine;

public class UIInteractionPrompt : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text label;

    private void Awake()
    {
        panel.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.onInteractionPromptChanged += UpdatePrompt;
    }

    private void OnDestroy()
    {
        GameEvents.onInteractionPromptChanged -= UpdatePrompt;
    }

    private void UpdatePrompt(LocalizedMessage prompt)
    {
        label.text = KeyGlyphs.Format(LocalizationManager.Instance.Resolve(prompt));
        panel.SetActive(!prompt.IsEmpty);
    }
}
