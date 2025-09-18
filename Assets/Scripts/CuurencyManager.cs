using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    private int gold = 0;

    [Header("UI Reference")]
    public TextMeshProUGUI goldText;  // drag your UI text here in inspector

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateGoldUI();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateGoldUI();

    }

    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            UpdateGoldUI();
            return true;
        }
        return false;
    }

    public int GetGold() => gold;

    private void UpdateGoldUI()
    {
        if (goldText != null)
            goldText.text = $"Gold: {gold}";
    }
}
