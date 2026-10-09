using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIResourceCounter : MonoBehaviour
{
    [SerializeField] private ResourceDefinition resource;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text valueLabel;

    private void Awake()
    {
        icon.sprite = resource.icon;
        icon.color = resource.color;
        valueLabel.text = "0";
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
            valueLabel.text = amount.ToString();
    }
}
