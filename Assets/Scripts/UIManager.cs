using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Page
{
    public string pageName;
    public GameObject pageObject;
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Inventory UI")]
    public Transform seedsPanel;      
    public Transform plantsPanel;     
    public InventoryItemUI itemPrefab; 
    
    public List<Page> pages; 
    private Dictionary<string, Page> pageLookup = new();

    private Dictionary<string, InventoryItemUI> activeItems = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        UpdateInventoryUI();

        foreach (var page in pages)
        {
            if(!pageLookup.ContainsKey(page.pageName))
                pageLookup.Add(page.pageName, page);
        }
    }

    public void UpdateInventoryUI()
    {
        var items = ItemManager.Instance.GetAllItems();

        // Instead of hiding all  just update existing
        foreach (var kvp in activeItems)
        {
            // If inventory has it  update
            if (items.ContainsKey(kvp.Key))
            {
                Sprite icon = ItemDatabase.Instance.GetIcon(kvp.Key);
                kvp.Value.SetData(icon, items[kvp.Key]);
                kvp.Value.gameObject.SetActive(true);
            }
            else
            {
                // Not in inventory anymore  keep but set count = 0
                Sprite icon = ItemDatabase.Instance.GetIcon(kvp.Key);
                kvp.Value.SetData(icon, 0);
                kvp.Value.gameObject.SetActive(true);
            }
        }

        // Add any new items not tracked yet
        foreach (var item in items)
        {
            if (!activeItems.ContainsKey(item.Key))
            {
                InventoryItemUI slot = Instantiate(itemPrefab, GetParentPanel(item.Key));
                activeItems[item.Key] = slot;

                Sprite icon = ItemDatabase.Instance.GetIcon(item.Key);
                slot.SetData(icon, item.Value);
                slot.gameObject.SetActive(true);
            }
        }
    }

    private Transform GetParentPanel(string itemName)
    {
        if (itemName.EndsWith("Seed"))
            return seedsPanel;
        else
            return plantsPanel;
    }

    public void ShowPage(string pageName)
    {
        // Implement showing different UI pages if needed
        foreach(var kvp in pageLookup)
        {

            kvp.Value.pageObject.SetActive(kvp.Key == pageName);

        }
    }
}
