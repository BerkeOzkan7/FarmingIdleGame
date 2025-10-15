using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class AutomationUpgrade
{
    public SeedData seed;
    public bool unlocked;
    public bool enabled;
    public int cost;

}
public class AutomationManager : MonoBehaviour
{
   
    public static AutomationManager Instance;

    public List<AutomationUpgrade> automations = new();

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Update()
    {
        foreach (var plot in PlotManager.Instance.plots)
        {
            if (!plot.isUnlocked) continue;

            // Auto-harvest if ready
            if (plot.hasPlant && plot.growthManager.IsFullyGrown())
            {
                SeedData seed = plot.lastPlantedSeed;
                // check if automation is enabled for this seed type
                if (IsAutomationEnabledFor(seed))
                {
                    plot.growthManager.Harvest();
                }
            }

            // Auto-replant and buy seeds if none are planted
            if (!plot.hasPlant && !string.IsNullOrEmpty(plot.lastPlantedSeed.seedName))
            {
                SeedData seed = plot.lastPlantedSeed;

                if (seed != null && IsAutomationEnabledFor(seed))
                {
                    if (ItemManager.Instance.HasEnough(seed.seedName, 1))
                    {
                        plot.Plant(seed);
                    }
                    else if (CurrencyManager.Instance.GetGold() >= seed.cost)
                    {
                        // Auto-buy 1 seed
                        CurrencyManager.Instance.SpendGold(seed.cost);
                        ItemManager.Instance.AddItem(seed.seedName, 1);
                        plot.Plant(seed);
                    }
                    else if (CurrencyManager.Instance.GetGold() < seed.cost)
                    {
                        // Sell 1 crop and buy seed
                        int level = UpgradeManager.Instance.GetUpgradeLevel(seed, UpgradeType.SellPrice) + 1;
                        ItemManager.Instance.RemoveItem(seed.cropName, 1);
                        CurrencyManager.Instance.AddGold(seed.sellPrice * level);
                        CurrencyManager.Instance.SpendGold(seed.cost);
                        ItemManager.Instance.AddItem(seed.seedName, 1);
                    }
                }
            }
            
            UIManager.Instance.UpdateInventoryUI();
        }
    }

    public bool IsAutomationEnabledFor(SeedData seed)
    {
        foreach(var auto in automations)
        {
            if(auto.seed == seed && auto.unlocked && auto.enabled)
            {
                return true;
            }
        }
        return false;
    }

    public void UnlockUpgrade(SeedData seed)
    {
        var auto = automations.Find(a => a.seed == seed);
        if (auto != null)
        {
            CurrencyManager.Instance.SpendGold(auto.cost);
            auto.unlocked = true;
        }

    }

    public void ToggleUpgrade(SeedData seed)
    {
        var auto = automations.Find(a => a.seed == seed);   
        if(auto != null && auto.unlocked)
        {
            auto.enabled = !auto.enabled;
        }
    }

    public bool IsUnlocked(SeedData seed)
    {
        var auto = automations.Find(a => a.seed == seed);
        return auto != null && auto.unlocked;
    }

    public bool IsEnabled(SeedData seed)
    {
        var auto = automations.Find(a => a.seed == seed);
        return auto != null && auto.enabled;
    }

}
