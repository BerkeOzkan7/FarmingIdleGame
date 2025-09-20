using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellBuyAmountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private Button cycleButton;
    public static SellBuyAmountManager Instance;

    private int[] options = { 1, 5, 10, 100, -1 }; // -1 means Max
    private int currentIndex = 0;

    public int CurrentAmount
    {
        get { return options[currentIndex]; }
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        cycleButton.onClick.AddListener(ChangeAmount);
        UpdateLabel();
    }

    private void ChangeAmount()
    {
        currentIndex = (currentIndex + 1) % options.Length;
        UpdateLabel();
    }
    private void UpdateLabel()
    {
        int value = options[currentIndex];
        amountText.text = value > 0 ? $"x{value}" : "Max";
    }

}
 