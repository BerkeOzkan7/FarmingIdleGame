using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text buySeedDescription;
    public TMP_Text sellCropDescription;
    public TMP_Text buySeedCostText;
    public TMP_Text sellCropPriceText;
    public Button buyButton;
    public Button sellButton;

    private SeedData seed;

    public void Setup(SeedData seedData)
    {
        seed = seedData;
        icon.sprite = seed.icon;
        buySeedDescription.text = seed.buySeedDescription;
        buySeedCostText.text = seed.cost.ToString() + " gold";

        sellCropDescription.text = seed.sellCropDescription;
        sellCropPriceText.text = seed.sellPrice.ToString() + " gold";

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuySeed);

        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(sellCrop);   
    }

    private void BuySeed()
    {
        
        if (CurrencyManager.Instance.GetGold() >= seed.cost)
        {
            CurrencyManager.Instance.SpendGold(seed.cost);
            ItemManager.Instance.AddItem(seed.seedName, 1);
            Debug.Log($"Bought {seed.seedName}");
        }
    }


    private void sellCrop()
    {
        if (ItemManager.Instance.HasEnough(seed.cropName, 1))
        {
            ItemManager.Instance.RemoveItem(seed.cropName, 1);
            CurrencyManager.Instance.AddGold(seed.sellPrice);
            UIManager.Instance.UpdateInventoryUI();
        }
        else
        {
            Debug.Log($"No {seed.cropName} to sell!");
        }
    }
}
