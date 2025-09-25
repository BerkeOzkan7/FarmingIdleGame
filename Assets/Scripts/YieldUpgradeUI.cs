using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class YieldUpgradeUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text label;
    public TMP_Text costText;
    public Button buyButton;

    private SeedData seed;

    public void Setup(SeedData seedData)
    {
        seed = seedData;
        icon.sprite = seedData.icon;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnBuyClicked);

        Refresh();
    }

    private void Refresh()
    {
        int level = UpgradeManager.Instance.GetUpgradeLevel(seed);
        int cost = UpgradeManager.Instance.GetUpgradeCost(seed);

        label.text = $"{seed.seedName} Yield Lv {level}";
        costText.text = $"Cost: {cost}";
    }

    private void OnBuyClicked()
    {
        UpgradeManager.Instance.BuyYieldUpgrade(seed);
        Refresh();
    }
}
