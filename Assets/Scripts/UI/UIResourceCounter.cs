using UnityEngine;
using UnityEngine.UI;

public class UIResourceCounter : MonoBehaviour
{
    [SerializeField] private ResourceDefinition resource;
    [SerializeField] private Text label;

    private void Awake()
    {
        UpdateLabel(0);
    }

    private void OnEnable()
    {
        GameEvents.onPlayerResourceChanged += UpdateResource;
    }

    private void OnDestroy()
    {
        GameEvents.onPlayerResourceChanged -= UpdateResource;
    }

    private void UpdateResource(ResourceDefinition changed, int amount, int delta)
    {
        if (changed == resource)
            UpdateLabel(amount);
    }

    private void UpdateLabel(int amount)
    {
        label.text = $"{LocalizationManager.Instance.Get(resource.displayName)}  {amount}";
        label.color = resource.color;
    }
}
