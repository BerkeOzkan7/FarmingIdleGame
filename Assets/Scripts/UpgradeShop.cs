using UnityEngine;

public class UpgradeShop : MonoBehaviour
{
    public SeedData[] availableSeeds;
    public YieldUpgradeUI upgradeItemPrefab;
    public Transform contentParent;

    public void Start()
    {
        PopulateShop();
    }

    public void PopulateShop()
    {
        foreach (var upgrade in availableSeeds)
        {
            YieldUpgradeUI ui = Instantiate(upgradeItemPrefab, contentParent);
            ui.Setup(upgrade);
        }
    }
}
