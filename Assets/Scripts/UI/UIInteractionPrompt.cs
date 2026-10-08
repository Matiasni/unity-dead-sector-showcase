using UnityEngine;
using UnityEngine.UI;

public class UIInteractionPrompt : MonoBehaviour
{
    [SerializeField] private Text label;

    private void Awake()
    {
        label.enabled = false;
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
        label.text = LocalizationManager.Instance.Resolve(prompt);
        label.enabled = !prompt.IsEmpty;
    }
}
