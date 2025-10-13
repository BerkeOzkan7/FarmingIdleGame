using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text seedNameText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Image seedIcon;

    private UpgradeData upgradeData;

    private void OnEnable()
    {
        SellBuyAmountManager.OnAmountChanged += OnAmountChanged;
    }

    private void OnDisable()
    {
        SellBuyAmountManager.OnAmountChanged -= OnAmountChanged;
    }

    public void Setup(UpgradeData data)
    {
        upgradeData = data;
        seedNameText.text = data.seed.seedName;
        seedIcon.sprite = data.seed.icon;
        UpdateUI();

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() =>
        {
            UpgradeManager.Instance.BuyUpgrade(data.seed, data.type);
            UpdateUI();
        });
    }

    private void UpdateUI()
    {
        int level = UpgradeManager.Instance.GetUpgradeLevel(upgradeData.seed, upgradeData.type);
        int amount = SellBuyAmountManager.CurrentAmount;

        // Calculate cost for display
        int cost;
        if (amount == -1) // Max
        {
            // Show next level cost for simplicity
            cost = UpgradeManager.Instance.GetUpgradeCost(upgradeData.seed, upgradeData.type);
            costText.text = $"Cost: {cost}+ (Max)";
        }
        else
        {
            int totalCost = 0;
            int tempLevel = level;
            for (int i = 0; i < amount; i++)
            {
                totalCost += Mathf.RoundToInt(upgradeData.baseCost * Mathf.Pow(upgradeData.costMultiplier, tempLevel));
                tempLevel++;
            }
            cost = totalCost;
            costText.text = $"Cost: {cost}";
        }

        levelText.text = $"Level: {level}";
    }

    private void OnAmountChanged(int newAmount)
    {
        UpdateUI();
    }
}
