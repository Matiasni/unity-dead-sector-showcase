using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponSlot : MonoBehaviour
{
    [SerializeField] private LayoutElement layout;
    [SerializeField] private Image background;
    [SerializeField] private Image keyBadge;
    [SerializeField] private TMP_Text keyLabel;
    [SerializeField] private Image icon;
    [SerializeField] private RectTransform details;
    [SerializeField] private TMP_Text nameLabel;
    [SerializeField] private TMP_Text magazineLabel;
    [SerializeField] private TMP_Text reserveLabel;

    [Header("Selected")]
    [SerializeField] private Sprite selectedBackground;
    [SerializeField] private Sprite selectedKeyBadge;
    [SerializeField] private Vector2 selectedSize = new(400f, 84f);
    [SerializeField] private Vector2 selectedIconSize = new(150f, 45f);
    [SerializeField] private Color selectedIconColor = UIPalette.Bone;
    [SerializeField] private Color selectedNameColor = UIPalette.Amber;

    [Header("Normal")]
    [SerializeField] private Sprite normalBackground;
    [SerializeField] private Sprite normalKeyBadge;
    [SerializeField] private Vector2 normalSize = new(360f, 64f);
    [SerializeField] private Vector2 normalIconSize = new(120f, 36f);
    [SerializeField] private Color normalIconColor = UIPalette.WithAlpha(UIPalette.Steel, 0.8f);
    [SerializeField] private Color normalNameColor = UIPalette.SteelLight;

    [Header("Ammo")]
    [SerializeField] private Color ammoColor = UIPalette.Bone;
    [SerializeField] private Color emptyAmmoColor = UIPalette.Danger;

    [Header("Animation")]
    [SerializeField] private float resizeDuration = 0.12f;
    [SerializeField] private float iconLeft = 52f;
    [SerializeField] private float detailsGap = 18f;

    private bool isSelected;
    private float blend;

    public void SetWeapon(int index, WeaponSettings weapon, string magazine, string reserve, bool isOutOfAmmo)
    {
        keyLabel.text = (index + 1).ToString();
        icon.sprite = weapon.icon;
        icon.enabled = weapon.icon != null;
        nameLabel.text = LocalizationManager.Instance.Get(weapon.weaponName);
        magazineLabel.text = magazine;
        magazineLabel.color = isOutOfAmmo ? emptyAmmoColor : ammoColor;
        reserveLabel.text = reserve;
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;

        background.sprite = selected ? selectedBackground : normalBackground;
        keyBadge.sprite = selected ? selectedKeyBadge : normalKeyBadge;
        keyLabel.color = selected ? UIPalette.Ink : UIPalette.Bone;
        icon.color = selected ? selectedIconColor : normalIconColor;
        nameLabel.color = selected ? selectedNameColor : normalNameColor;
    }

    public void SnapLayout()
    {
        blend = isSelected ? 1f : 0f;
        ApplyLayout();
    }

    private void Update()
    {
        float target = isSelected ? 1f : 0f;

        if (Mathf.Approximately(blend, target)) return;

        blend = Mathf.MoveTowards(blend, target, Time.unscaledDeltaTime / resizeDuration);
        ApplyLayout();
    }

    private void ApplyLayout()
    {
        Vector2 size = Vector2.Lerp(normalSize, selectedSize, blend);
        layout.preferredWidth = size.x;
        layout.preferredHeight = size.y;

        Vector2 iconSize = Vector2.Lerp(normalIconSize, selectedIconSize, blend);
        icon.rectTransform.sizeDelta = iconSize;
        icon.rectTransform.anchoredPosition = new Vector2(iconLeft, 0f);

        details.offsetMin = new Vector2(iconLeft + iconSize.x + detailsGap, details.offsetMin.y);
        magazineLabel.fontSize = Mathf.Lerp(22f, 30f, blend);
        nameLabel.fontSize = Mathf.Lerp(16f, 18f, blend);
    }
}
