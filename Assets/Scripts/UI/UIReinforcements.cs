using TMPro;
using UnityEngine;

public class UIReinforcements : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private UIPips pips;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color lastLifeColor = new(1f, 0.35f, 0.3f);

    private void OnEnable()
    {
        GameEvents.onReinforcementsChanged += UpdateReinforcements;
    }

    private void OnDestroy()
    {
        GameEvents.onReinforcementsChanged -= UpdateReinforcements;
    }

    private void UpdateReinforcements(int remaining)
    {
        pips.Show(remaining, remaining);
        label.color = remaining > 0 ? normalColor : lastLifeColor;
    }
}
