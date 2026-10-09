using UnityEngine;
using TMPro;

public class UIHealthNumbers : MonoBehaviour
{
    public TextMeshProUGUI text;

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
        text.text = $"{current}<size=65%><color=#969C9C> / {max}</color></size>";
    }
}
