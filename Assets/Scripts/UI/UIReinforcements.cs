using UnityEngine;
using UnityEngine.UI;

public class UIReinforcements : MonoBehaviour
{
    [SerializeField] private Text label;
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
        label.text = LocalizationManager.Instance.Resolve(new LocalizedMessage("Reinforcements  {0}", remaining));
        label.color = remaining > 0 ? normalColor : lastLifeColor;
    }
}
