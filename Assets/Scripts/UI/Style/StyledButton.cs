using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StyledButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private ButtonStyle style;
    [SerializeField] private TMP_Text label;

    private Button button;
    private bool isHovered;
    private bool isPressed;
    private bool wasInteractable;

    private Button Button
    {
        get
        {
            if (button == null)
                button = GetComponent<Button>();

            return button;
        }
    }

    private void Awake()
    {
        ApplyStyle();
    }

    private void OnDisable()
    {
        isHovered = false;
        isPressed = false;
        RefreshLabel();
    }

    private void LateUpdate()
    {
        if (wasInteractable != Button.IsInteractable())
            RefreshLabel();
    }

    public void SetStyle(ButtonStyle newStyle)
    {
        style = newStyle;
        ApplyStyle();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        RefreshLabel();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        isPressed = false;
        RefreshLabel();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        RefreshLabel();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        RefreshLabel();
    }

    private void ApplyStyle()
    {
        if (style == null) return;

        Button.transition = Selectable.Transition.SpriteSwap;
        Button.image.sprite = style.normal;
        Button.spriteState = new SpriteState
        {
            highlightedSprite = style.highlighted,
            pressedSprite = style.pressed,
            selectedSprite = style.normal,
            disabledSprite = style.disabled
        };

        RefreshLabel();
    }

    private void RefreshLabel()
    {
        wasInteractable = Button.IsInteractable();

        if (label == null || style == null) return;

        if (!wasInteractable)
            label.color = style.labelDisabled;
        else if (isPressed)
            label.color = style.labelPressed;
        else if (isHovered)
            label.color = style.labelHighlighted;
        else
            label.color = style.labelNormal;
    }

    private void OnValidate()
    {
        ApplyStyle();
    }
}
