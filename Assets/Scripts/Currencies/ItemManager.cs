using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    // Keeps counts of items (by name, e.g. "Corn")
    private Dictionary<string, int> items = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
      
        
    }

    private void Start()
    {
        AddItem("CornSeed", 1); // start with 1 corn seed 
    }

    public void AddItem(string itemName, int amount = 1)
    {
        if (!items.ContainsKey(itemName))
            items[itemName] = 0;

        items[itemName] += amount;
        UIManager.Instance.UpdateInventoryUI();
        Debug.Log($"Added {amount} {itemName}. Total: {items[itemName]}");
    }

    public bool HasEnough(string itemName, int required)
    {
        return items.ContainsKey(itemName) && items[itemName] >= required;
    }

    public void RemoveItem(string itemName, int amount)
    {
        if (items.ContainsKey(itemName))
        {
            items[itemName] -= amount;
            if (items[itemName] <= 0) items.Remove(itemName);
        }
    }



    public int GetCount(string itemName)
    {
        return items.ContainsKey(itemName) ? items[itemName] : 0;
    }

    public Dictionary<string, int> GetAllItems()
    {
        return items;
    }
}
