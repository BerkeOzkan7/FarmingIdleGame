using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeedButtonUI : MonoBehaviour
{
    public Image icon;
    private SeedData seed;
    private Plot targetPlot;

    public void Setup(SeedData seedData, Plot plot)
    {
        seed = seedData;
        targetPlot = plot;

        icon.sprite = seed.icon;

        Button btn = GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {

        Debug.Log($"Planting {seed.seedName} in selected plot.");
        if (targetPlot != null)
        {
            targetPlot.Plant(seed);
            PlantSelectionUI.Instance.Hide();
        }
    }
}
