using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Yield Upgrade")]
public class YieldUpgradeData : ScriptableObject
{
    public SeedData seed;          // Which crop this applies to
    public int baseCost = 10;      // Starting gold cost
    public float costMultiplier = 1.5f; // Cost scaling (e.g. each level *1.5)
    public int yieldPerLevel = 1;  // Extra yield added per upgrade level
}
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [SerializeField] private List<YieldUpgradeData> yieldUpgrades;
    private Dictionary<SeedData, int> yieldLevels = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Buy one level of upgrade
    public void BuyYieldUpgrade(SeedData seed)
    {
        var data = GetUpgradeData(seed);
        if (data == null) return;

        int level = GetUpgradeLevel(seed);
        int cost = Mathf.RoundToInt(data.baseCost * Mathf.Pow(data.costMultiplier, level));

        if (CurrencyManager.Instance.GetGold()>= cost)
        {
            CurrencyManager.Instance.SpendGold(cost);
            yieldLevels[seed] = level + 1;
            Debug.Log($"{seed.seedName} upgrade to level {level + 1}!");
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }

    // How much bonus this crop gets
    public int GetExtraYield(SeedData seed)
    {
        int level = GetUpgradeLevel(seed);
        var data = GetUpgradeData(seed);
        return level * (data != null ? data.yieldPerLevel : 0);
    }

    public int GetUpgradeLevel(SeedData seed)
    {
        return yieldLevels.TryGetValue(seed, out int lvl) ? lvl : 0;
    }

    public int GetUpgradeCost(SeedData seed)
    {
        var data = GetUpgradeData(seed);
        if (data == null) return 0;

        int level = GetUpgradeLevel(seed);
        return Mathf.RoundToInt(data.baseCost * Mathf.Pow(data.costMultiplier, level));
    }

    private YieldUpgradeData GetUpgradeData(SeedData seed)
    {
        return yieldUpgrades.Find(u => u.seed == seed);
    }
}
