using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAbilitySlot : MonoBehaviour
{
    [SerializeField] private Image frame;
    [SerializeField] private Image icon;
    [SerializeField] private Image cooldownMask;
    [SerializeField] private TMP_Text cooldownLabel;
    [SerializeField] private TMP_Text chargesLabel;
    [SerializeField] private UIPips chargePips;
    [SerializeField] private Image readyGlow;
    [SerializeField] private int maxPips = 5;

    [Header("Frames")]
    [SerializeField] private Sprite readyFrame;
    [SerializeField] private Sprite cooldownFrame;
    [SerializeField] private Sprite activeFrame;
    [SerializeField] private Sprite emptyFrame;

    [Header("Feedback")]
    [SerializeField] private Color activeIconColor = UIPalette.Cyan;
    [SerializeField] private float dimmedAlpha = 0.6f;
    [SerializeField] private float glowDuration = 0.35f;

    private IAbility ability;
    private bool wasCoolingDown;
    private float glowElapsed = -1f;

    public void Bind(IAbility ability)
    {
        this.ability = ability;

        bool hasIcon = ability != null && ability.Definition.icon != null;
        icon.enabled = hasIcon;

        if (hasIcon)
            icon.sprite = ability.Definition.icon;

        wasCoolingDown = false;
        Refresh();
    }

    private void Update()
    {
        Refresh();
        AnimateGlow();
    }

    private void Refresh()
    {
        if (ability == null)
        {
            frame.sprite = emptyFrame;
            cooldownMask.fillAmount = 0f;
            cooldownLabel.text = string.Empty;
            chargesLabel.text = string.Empty;
            chargePips.Show(0, 0);
            return;
        }

        float cooldown = Mathf.Clamp01(ability.CooldownNormalized);
        bool isCoolingDown = cooldown > 0f;
        bool isActive = ability.ActiveProgress >= 0f;
        bool isEmpty = ability.HasCharges && ability.Charges <= 0;

        if (isActive)
            frame.sprite = activeFrame;
        else if (isCoolingDown)
            frame.sprite = cooldownFrame;
        else
            frame.sprite = isEmpty ? emptyFrame : readyFrame;

        cooldownMask.fillAmount = cooldown;
        cooldownLabel.text = isCoolingDown ? (cooldown * ability.Definition.cooldown).ToString("0.0") : string.Empty;

        Color tint = isActive ? activeIconColor : ability.Definition.uiColor;
        tint.a = isCoolingDown || isEmpty ? dimmedAlpha : 1f;
        icon.color = tint;

        RefreshCharges();

        if (wasCoolingDown && !isCoolingDown)
            glowElapsed = 0f;

        wasCoolingDown = isCoolingDown;
    }

    private void RefreshCharges()
    {
        if (!ability.HasCharges)
        {
            chargesLabel.text = string.Empty;
            chargePips.Show(0, 0);
            return;
        }

        bool usePips = ability.MaxCharges <= maxPips;

        chargePips.Show(usePips ? ability.Charges : 0, usePips ? ability.MaxCharges : 0);
        chargesLabel.text = usePips ? string.Empty : $"{ability.Charges}/{ability.MaxCharges}";
    }

    private void AnimateGlow()
    {
        if (glowElapsed < 0f)
        {
            readyGlow.enabled = false;
            return;
        }

        glowElapsed += Time.unscaledDeltaTime;
        float t = glowElapsed / glowDuration;

        if (t >= 1f)
        {
            glowElapsed = -1f;
            readyGlow.enabled = false;
            return;
        }

        readyGlow.enabled = true;
        readyGlow.rectTransform.localScale = Vector3.one * Mathf.Lerp(1f, 1.15f, t);

        Color color = readyGlow.color;
        color.a = 1f - t;
        readyGlow.color = color;
    }
}
