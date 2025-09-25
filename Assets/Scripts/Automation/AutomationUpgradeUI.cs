using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutomationUpgradeUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text label;
    public Button unlockButton;
    public ToggleButton toggle;
    public TMP_Text costText;
    

    private SeedData seed;

    public void Setup(SeedData seedData)
    {
        seed = seedData;
        icon.sprite = seed.icon;
        label.text = $"{seed.seedName} Automation";

        // set initial state
        bool isUnlocked = AutomationManager.Instance.IsUnlocked(seed);
        toggle.gameObject.SetActive(isUnlocked);
        unlockButton.gameObject.SetActive(!isUnlocked);
        costText.text = AutomationManager.Instance.automations.Find(a => a.seed == seed).cost.ToString() + " gold";

        if (isUnlocked)
        {
            bool isEnabled = AutomationManager.Instance.IsEnabled(seed);
            toggle.SetState(isEnabled);
        }

        // listeners
        unlockButton.onClick.RemoveAllListeners();
        unlockButton.onClick.AddListener(OnUnlockClicked);
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnUnlockClicked()
    {
        AutomationManager.Instance.UnlockUpgrade(seed);

        unlockButton.gameObject.SetActive(false);
        toggle.gameObject.SetActive(true);
        toggle.SetState(false); // default off
    }

    private void OnToggleChanged(bool value)
    {
        AutomationManager.Instance.ToggleUpgrade(seed);
    }
}
