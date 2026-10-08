using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIPlayerHealth : MonoBehaviour
{
    public Slider healthSlider;

    public UnityEvent UnityEvent;

    private void OnEnable()
    {
        GameEvents.onPlayerHealthChanged += UpdateHealth;
    }

    private void OnDestroy()
    {
        GameEvents.onPlayerHealthChanged -= UpdateHealth;
    }

    private void UpdateHealth(int current, int max)
    {
        healthSlider.value = (float)current / max;
        UnityEvent?.Invoke();
    }

    private void Update()
    {
        GameEvents.OnUdate(Time.deltaTime);
    }
}