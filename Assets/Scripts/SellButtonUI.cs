using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellButtonUI : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text priceText;
    public Button sellButton;

    private CropData cropData;

    public void Setup(CropData data)
    {
        cropData = data;
        iconImage.sprite = data.icon;
        nameText.text = data.cropName;
        priceText.text = $"Sell: {data.sellPrice}";

        sellButton.onClick.AddListener(SellCrop);
    }

    private void SellCrop()
    {
        if (ItemManager.Instance.HasEnough(cropData.cropName, 1))
        {
            ItemManager.Instance.RemoveItem(cropData.cropName, 1);
            CurrencyManager.Instance.AddGold(cropData.sellPrice);
            UIManager.Instance.UpdateInventoryUI();
        }
        else
        {
            Debug.Log($"No {cropData.cropName} to sell!");
        }
    }
}