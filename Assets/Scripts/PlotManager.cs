using UnityEngine;
using System.Collections.Generic;

public class PlotManager : MonoBehaviour
{
    public static PlotManager Instance { get; private set; }

    public List<Plot> plots;  // assign all 16 in inspector
    public int startingUnlocked = 1;



    private Plot selectedPlot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        // Unlock the first X plots
        for (int i = 0; i < startingUnlocked; i++)
            plots[i].UnlockPlot();

        // For testing, plant corn in the first plot
     
    }

    public void SelectEmptyPlot(Plot plot)
    {
        selectedPlot = plot;
        Debug.Log("Selected empty plot!");
        // Show UI popup here
        
    }

    public void PlantSelected(SeedData seed, Plot targetPlot)
    {
        targetPlot.Plant(seed);
    }


    public void BuyPlotWithItems(int index, string requiredItem, int requiredAmount)
    {
        if (!plots[index].isUnlocked && ItemManager.Instance.HasEnough(requiredItem, requiredAmount))
        {
            ItemManager.Instance.RemoveItem(requiredItem, requiredAmount);
            plots[index].UnlockPlot();
            Debug.Log($"Unlocked plot {index} with {requiredAmount} {requiredItem}!");
        }
        else
        {
            Debug.Log("Not enough items to unlock this plot!");
        }
    }
}
