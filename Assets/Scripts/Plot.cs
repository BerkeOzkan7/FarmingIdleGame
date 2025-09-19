using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Plot : MonoBehaviour
{
    public bool isUnlocked = false;
    public bool hasPlant = false;
    public GrowthManager growthManager;

    [Header("Unlock Settings")]
    public GameObject lockedOverlay; 
    public TMP_Text requiredCropsText;
    public string requiredItem = "Corn";
    public int requiredAmount = 1;


    private void Awake()
    {
        // Get the button(s) inside this plot prefab
        Button[] childButtons = GetComponentsInChildren<Button>();

        foreach (Button btn in childButtons)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(OnClickPlot); 

        }
        requiredCropsText.text = $" x{requiredAmount} {requiredItem} to unlock";
    }

    public void UnlockPlot()
    {
        isUnlocked = true;
        if (lockedOverlay != null) lockedOverlay.SetActive(false);
        Debug.Log("Plot unlocked!");
    }

    public void Plant(SeedData seed)
    {
        if (!isUnlocked || hasPlant) return;

        // check inventory
        if (!ItemManager.Instance.HasEnough(seed.seedName, 1))
        {
            Debug.Log($"Not enough {seed.seedName} to plant!");
            return;
        }

        // remove one seed
        ItemManager.Instance.RemoveItem(seed.seedName, 1);
        UIManager.Instance.UpdateInventoryUI();

        // spawn plant prefab
        GrowthManager newPlant = Instantiate(seed.plantPrefab, transform).GetComponent<GrowthManager>();
        growthManager = newPlant;
        hasPlant = true;

        // position reset (UI scaling stuff)
        RectTransform rt = newPlant.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.localScale = Vector3.one;
            rt.anchoredPosition = Vector2.zero;
        }

        growthManager.StartGrowth();

        Debug.Log($"Planted {seed.seedName}!");
    }

    public void OnClickPlot()
    {

        Debug.Log("Plot clicked at: " + Input.mousePosition);
        if (!isUnlocked)
        {
            TryUnlock();
            return;
        }

        if (!hasPlant)
        {
            // Tell manager we clicked an empty plot
            PlantSelectionUI.Instance.Show(this);
            
            PlotManager.Instance.SelectEmptyPlot(this);

        }
        else
        {
            // Forward click to plant (to speed up growth)
            growthManager.SpeedUp(1f);
            growthManager.Harvest();
        }
    }

    private void TryUnlock()
    {
        if (ItemManager.Instance.HasEnough(requiredItem, requiredAmount))
        {
            ItemManager.Instance.RemoveItem(requiredItem, requiredAmount);
            UIManager.Instance.UpdateInventoryUI();
            UnlockPlot();
        }
        else
        {
            Debug.Log($"Need {requiredAmount} {requiredItem} to unlock!");
            // Optional: show UI popup here
        }
    }


    public void ClearPlot()
    {
        growthManager = null;
        hasPlant = false;
    }
}
