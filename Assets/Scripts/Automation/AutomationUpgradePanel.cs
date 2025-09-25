using UnityEngine;

public class AutomationUpgradePanel : MonoBehaviour
{
    [SerializeField] private AutomationUpgradeUI upgradePrefab;
    [SerializeField] private Transform contentParent; // e.g., Vertical Layout Group container
    [SerializeField] private SeedData[] allSeeds;

    private void Start()
    {
        foreach (var seed in allSeeds)
        {
            AutomationUpgradeUI ui = Instantiate(upgradePrefab, contentParent);
            ui.Setup(seed);
        }
    }
}