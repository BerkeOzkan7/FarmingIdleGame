using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellBuyAmountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private Button cycleButton;

    private static int[] options = { 1, 5, 10, 100, -1 };
    private static int currentIndex = 0;

    public static int CurrentAmount { get; private set; } = 1;

    private static List<SellBuyAmountManager> allSelectors = new();

    public static event Action<int> OnAmountChanged;

    private void Awake()
    {
        allSelectors.Add(this);
        cycleButton.onClick.AddListener(ChangeAmount);
        UpdateLabel();
    }

    private void OnDestroy()
    {
        allSelectors.Remove(this);
    }

    private void ChangeAmount()
    {
        currentIndex = (currentIndex + 1) % options.Length;
        UpdateAllLabels();
        OnAmountChanged?.Invoke(CurrentAmount);
    }

    private void UpdateLabel()
    {
        int value = options[currentIndex];
        CurrentAmount = value;
        amountText.text = value > 0 ? $"x{value}" : "Max";
    }

    private static void UpdateAllLabels()
    {
        foreach (var selector in allSelectors)
        {
            selector.UpdateLabel();
        }
    }
}
