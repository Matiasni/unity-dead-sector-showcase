using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeChoice : MonoBehaviour
{
    [SerializeField] private Button[] optionButtons;
    [SerializeField] private TMP_Text[] titleLabels;
    [SerializeField] private TMP_Text[] descriptionLabels;
    [SerializeField] private TMP_Text[] categoryLabels;

    private UpgradeDefinition[] options = new UpgradeDefinition[0];

    private void OnEnable()
    {
        GameEvents.onUpgradesOffered += ShowOptions;
    }

    private void OnDestroy()
    {
        GameEvents.onUpgradesOffered -= ShowOptions;
    }

    private void ShowOptions(UpgradeDefinition[] options)
    {
        this.options = options;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            bool hasOption = i < options.Length;
            optionButtons[i].gameObject.SetActive(hasOption);

            if (!hasOption) continue;

            titleLabels[i].text = LocalizationManager.Instance.Get(options[i].upgradeName);
            descriptionLabels[i].text = LocalizationManager.Instance.Get(options[i].description);
            categoryLabels[i].text = LocalizationManager.Instance.Get(options[i].category.ToString().ToUpper());
        }

        UIEvents.SetScreen(ScreenType.UpgradeChoice, true);
    }

    public void Choose(int index)
    {
        if (index < 0 || index >= options.Length) return;

        var chosen = options[index];
        options = new UpgradeDefinition[0];

        UIEvents.SetScreen(ScreenType.UpgradeChoice, false);
        GameEvents.OnUpgradeSelected(chosen);
    }
}
