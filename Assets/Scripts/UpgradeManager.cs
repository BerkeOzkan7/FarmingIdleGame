using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    Yield,
    Speed,
    SellPrice
}

[System.Serializable]
[CreateAssetMenu(menuName = "Upgrades/Generic Upgrade")]
public class UpgradeData : ScriptableObject
{
    public UpgradeType type;
    public SeedData seed;
    public int baseCost = 10;
    public float costMultiplier = 1.5f;

    public int yieldPerLevel = 0;
    public float speedPerLevel = 0f;
    public float sellPricePerLevel = 0f;
}

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [SerializeField] private List<UpgradeData> allUpgrades;
    private Dictionary<(SeedData, UpgradeType), int> upgradeLevels = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void BuyUpgrade(SeedData seed, UpgradeType type)
    {
        int levelsBought = 0;
        int amount = SellBuyAmountManager.CurrentAmount;
        var data = GetUpgradeData(seed, type);
        if (data == null) return;

        if (amount == -1) // Max
        {
            while (true)
            {
                int level = GetUpgradeLevel(seed, type);
                int cost = Mathf.RoundToInt(data.baseCost * Mathf.Pow(data.costMultiplier, level));

                if (CurrencyManager.Instance.GetGold() >= cost)
                {
                    CurrencyManager.Instance.SpendGold(cost);
                    upgradeLevels[(seed, type)] = level + 1;
                    levelsBought++;
                }
                else break;
            }
        }
        else
        {
            for (int i = 0; i < amount; i++)
            {
                int level = GetUpgradeLevel(seed, type);
                int cost = Mathf.RoundToInt(data.baseCost * Mathf.Pow(data.costMultiplier, level));

                if (CurrencyManager.Instance.GetGold() >= cost)
                {
                    CurrencyManager.Instance.SpendGold(cost);
                    upgradeLevels[(seed, type)] = level + 1;
                    levelsBought++;
                }
                else break;
            }
        }

        Debug.Log(levelsBought > 0
            ? $"{seed.seedName} {type} upgraded {levelsBought} times. New level: {GetUpgradeLevel(seed, type)}"
            : "Not enough gold!");
    }

    public int GetUpgradeLevel(SeedData seed, UpgradeType type)
    {
        return upgradeLevels.TryGetValue((seed, type), out int lvl) ? lvl : 0;
    }

    public int GetUpgradeCost(SeedData seed, UpgradeType type)
    {
        var data = GetUpgradeData(seed, type);
        if (data == null) return 0;
        int level = GetUpgradeLevel(seed, type);
        return Mathf.RoundToInt(data.baseCost * Mathf.Pow(data.costMultiplier, level));
    }

    public float GetBonusValue(SeedData seed, UpgradeType type)
    {
        int level = GetUpgradeLevel(seed, type);
        var data = GetUpgradeData(seed, type);
        if (data == null) return 0;

        return type switch
        {
            UpgradeType.Yield => level * data.yieldPerLevel,
            UpgradeType.Speed => level * data.speedPerLevel,
            UpgradeType.SellPrice => level * data.sellPricePerLevel,
            _ => 0
        };
    }

    private UpgradeData GetUpgradeData(SeedData seed, UpgradeType type)
    {
        return allUpgrades.Find(u => u.seed == seed && u.type == type);
    }

    public List<UpgradeData> GetUpgradesOfType(UpgradeType type)
    {
        return allUpgrades.FindAll(u => u.type == type);
    }
}
