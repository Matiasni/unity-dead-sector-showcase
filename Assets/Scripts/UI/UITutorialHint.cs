using UnityEngine;
using UnityEngine.UI;

public class UITutorialHint : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private Text label;
    [SerializeField] private float fadeSpeed = 3f;

    private float hideTime;

    private void Awake()
    {
        group.alpha = 0f;
    }

    private void OnEnable()
    {
        GameEvents.onTutorialHint += Show;
    }

    private void OnDestroy()
    {
        GameEvents.onTutorialHint -= Show;
    }

    private void Show(string hint, float duration)
    {
        label.text = LocalizationManager.Instance.Get(hint);
        hideTime = Time.unscaledTime + duration;
    }

    private void Update()
    {
        float target = Time.unscaledTime < hideTime ? 1f : 0f;
        group.alpha = Mathf.MoveTowards(group.alpha, target, fadeSpeed * Time.unscaledDeltaTime);
    }
}
