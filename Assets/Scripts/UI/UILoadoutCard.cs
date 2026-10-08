using UnityEngine;
using UnityEngine.UI;

public class UILoadoutCard : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Text titleLabel;
    [SerializeField] private Text descriptionLabel;
    [SerializeField] private Color selectedColor = new(0.25f, 0.55f, 0.3f, 0.95f);
    [SerializeField] private Color idleColor = new(0.15f, 0.15f, 0.18f, 0.9f);

    public void Setup(string title, string description)
    {
        titleLabel.text = title;
        descriptionLabel.text = description;
    }

    public void SetSelected(bool selected)
    {
        background.color = selected ? selectedColor : idleColor;
    }
}
