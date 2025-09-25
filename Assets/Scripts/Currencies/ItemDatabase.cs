using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ItemIcon
{
    public string itemName;
    public Sprite icon;
}

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;

    public List<ItemIcon> itemIcons = new();
    private Dictionary<string, Sprite> lookup = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        foreach (var entry in itemIcons)
        {
            if (!lookup.ContainsKey(entry.itemName))
                lookup.Add(entry.itemName, entry.icon);
        }
    }

    public Sprite GetIcon(string itemName)
    {
        return lookup.ContainsKey(itemName) ? lookup[itemName] : null;
    }
}
