using UnityEngine;


[CreateAssetMenu(fileName = "NewSeed", menuName = "Farming/Seed")]
public class SeedData : ScriptableObject
{
    public string seedName;
    public string buySeedDescription;
    public string sellCropDescription; 
    public Sprite icon;
    public int cost;
    public float growTime;   
    public string cropName;  
    public int sellPrice; 
    public GameObject plantPrefab;
}
public class SeedShop : MonoBehaviour
{
    public SeedData[] availableSeeds;        // drag ScriptableObjects here
    public GameObject shopItemPrefab;        // assign your ShopItemUI prefab
    public Transform contentParent;          // assign BuySeedPage/Content

    private void Start()
    {
        PopulateShop();
    }

    private void PopulateShop()
    {
        foreach (SeedData seed in availableSeeds)
        {
            GameObject entry = Instantiate(shopItemPrefab, contentParent);
            ShopItemUI ui = entry.GetComponent<ShopItemUI>();
            ui.Setup(seed);
        }
    }
}
