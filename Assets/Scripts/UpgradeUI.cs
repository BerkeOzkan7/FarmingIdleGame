using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private Transform contentPanel;
    [SerializeField] private GameObject upgradeItemPrefab;

    private UpgradeType currentType;

    public void ShowUpgrades(UpgradeType type)
    {
        currentType = type;

        // Clear old items
        foreach (Transform child in contentPanel)
            Destroy(child.gameObject);

        // Spawn new ones
        List<UpgradeData> upgrades = UpgradeManager.Instance.GetUpgradesOfType(type);

        foreach (var upgrade in upgrades)
        {
            GameObject item = Instantiate(upgradeItemPrefab, contentPanel);
            item.GetComponent<UpgradeItemUI>().Setup(upgrade);
        }
    }
    public void ShowYieldUpgrades()
    {
        ShowUpgrades(UpgradeType.Yield);
    }

    public void ShowSpeedUpgrades()
    {
        ShowUpgrades(UpgradeType.Speed);
    }

    public void ShowSellPriceUpgrades()
    {
        ShowUpgrades(UpgradeType.SellPrice);
    }
}
