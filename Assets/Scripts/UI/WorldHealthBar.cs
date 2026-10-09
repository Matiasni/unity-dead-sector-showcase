using UnityEngine;
using UnityEngine.UI;

public class WorldHealthBar : MonoBehaviour
{
    [SerializeField] private HealthBehaviour health;
    [SerializeField] private CanvasGroup group;
    [SerializeField] private Image fill;
    [SerializeField] private float visibleTime = 2f;
    [SerializeField] private float fadeSpeed = 4f;

    private float lastChangeTime = float.NegativeInfinity;

    private void OnEnable()
    {
        health.OnHealthChanged += HandleHealthChanged;
        group.alpha = 0f;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(int current, int max)
    {
        fill.fillAmount = max > 0 ? (float)current / max : 0f;
        lastChangeTime = Time.time;
    }

    private void Update()
    {
        bool isHurt = health.CurrentHealth < health.MaxHealth;
        bool isVisible = health.IsAlive && (isHurt || Time.time - lastChangeTime < visibleTime);

        group.alpha = Mathf.MoveTowards(group.alpha, isVisible ? 1f : 0f, fadeSpeed * Time.deltaTime);
    }
}
