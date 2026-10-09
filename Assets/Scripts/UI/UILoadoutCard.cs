using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILoadoutCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image background;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text titleLabel;
    [SerializeField] private TMP_Text descriptionLabel;

    [Header("Sprites")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite hoverSprite;
    [SerializeField] private Sprite selectedSprite;

    [Header("Icon")]
    [SerializeField] private Color iconColor = UIPalette.Bone;
    [SerializeField] private Color selectedIconColor = UIPalette.Amber;

    private bool isSelected;
    private bool isHovered;

    public void Setup(string title, string description, Sprite iconSprite)
    {
        titleLabel.text = title;
        descriptionLabel.text = description;

        icon.sprite = iconSprite;
        icon.enabled = iconSprite != null;
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        Refresh();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        Refresh();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        Refresh();
    }

    private void Refresh()
    {
        if (isSelected)
            background.sprite = selectedSprite;
        else
            background.sprite = isHovered ? hoverSprite : normalSprite;

        icon.color = isSelected ? selectedIconColor : iconColor;
    }
}
