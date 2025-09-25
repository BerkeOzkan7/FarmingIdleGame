using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.Universal.PixelPerfectCamera;

public class ShopItemUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text buySeedDescription;
    public TMP_Text sellCropDescription;
    public TMP_Text buySeedCostText;
    public TMP_Text sellCropPriceText;
    public Button buyButton;
    public Button sellButton;
    public static ShopItemUI Instance;
    private SeedData seed;


    public void Setup(SeedData seedData)
    {

        int amount = SellBuyAmountManager.Instance.CurrentAmount;
        seed = seedData;
        icon.sprite = seed.icon;
        buySeedDescription.text = seed.buySeedDescription;
        buySeedCostText.text = (seed.cost * amount).ToString() + " gold";

        sellCropDescription.text = seed.sellCropDescription;
        sellCropPriceText.text = (seed.sellPrice * amount).ToString() + " gold";

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuySeed);

        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(sellCrop);   
    }

    private void Update()
    {
        UpdatePrices();
    }

    public void UpdatePrices()
    {
        
        int amount = SellBuyAmountManager.Instance.CurrentAmount;
        if(amount == -1)
        {
            int maxAffordable = CurrencyManager.Instance.GetGold() / seed.cost;     
            buySeedCostText.text = (seed.cost * maxAffordable).ToString() + " gold";
            int seedCount = ItemManager.Instance.GetCount(seed.cropName);
            sellCropPriceText.text = (seed.sellPrice * seedCount).ToString() + " gold";

        }
        else
        {
            buySeedCostText.text = (seed.cost * amount).ToString() + " gold";
            sellCropPriceText.text = (seed.sellPrice * amount).ToString() + " gold";
        }

    }

    private void BuySeed()
    {

        int amount = SellBuyAmountManager.Instance.CurrentAmount;

        if (amount == -1)
        {
            int maxAffordable = CurrencyManager.Instance.GetGold() / seed.cost;
            amount = maxAffordable;
        }

        if (amount <= 0) return;

        int totalCost = seed.cost * amount;

        if (CurrencyManager.Instance.GetGold() >= totalCost)
        {
            CurrencyManager.Instance.SpendGold(totalCost);
            ItemManager.Instance.AddItem(seed.seedName, amount);
            Debug.Log($"Bought {seed.seedName},{amount}");
        }
    }


    private void sellCrop()
    {

        int amount = SellBuyAmountManager.Instance.CurrentAmount;

        if (amount == -1)
        {
            amount = ItemManager.Instance.GetCount(seed.cropName);
        }

        if (amount <= 0) return;

        if (ItemManager.Instance.HasEnough(seed.cropName, amount))
        {
            ItemManager.Instance.RemoveItem(seed.cropName, amount);
            CurrencyManager.Instance.AddGold(seed.sellPrice * amount);
            UIManager.Instance.UpdateInventoryUI();
            Debug.Log($"Sold {amount} {seed.cropName} for {seed.sellPrice * amount} gold.");
        }
        else
        {
            Debug.Log($"No {seed.cropName} to sell!");
        }
    }
}
