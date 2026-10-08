using UnityEngine;
using UnityEngine.UI;

public class UIAbilitySlot : MonoBehaviour
{
    [SerializeField] private Text nameLabel;
    [SerializeField] private Text chargesLabel;
    [SerializeField] private Image background;
    [SerializeField] private RectTransform cooldownOverlay;
    [SerializeField] private RectTransform activeBar;
    [SerializeField] private Color emptyColor = new(0.1f, 0.1f, 0.1f, 0.8f);

    private IAbility ability;

    public void Bind(IAbility ability)
    {
        this.ability = ability;

        nameLabel.text = ability != null ? LocalizationManager.Instance.Get(ability.Definition.abilityName) : "-";
        background.color = ability != null ? ability.Definition.uiColor : emptyColor;

        Refresh();
    }

    private void Update()
    {
        Refresh();
    }

    private void Refresh()
    {
        float cooldown = ability != null ? Mathf.Clamp01(ability.CooldownNormalized) : 0f;
        cooldownOverlay.anchorMax = new Vector2(1f, cooldown);

        float active = ability != null ? ability.ActiveProgress : -1f;
        activeBar.gameObject.SetActive(active >= 0f);
        activeBar.anchorMax = new Vector2(Mathf.Clamp01(active), activeBar.anchorMax.y);

        chargesLabel.text = ability != null && ability.HasCharges
            ? $"{ability.Charges}/{ability.MaxCharges}"
            : string.Empty;
    }
}
