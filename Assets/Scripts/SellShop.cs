using UnityEditor.ShaderGraph;
using UnityEngine;


[CreateAssetMenu(menuName = "Farming/SellShop")]


public class SellShop : MonoBehaviour
{
    public CropData[] availableCrops;       // assign in inspector
    public GameObject sellButtonPrefab;     // prefab with icon, name, price, button
    public Transform contentPanel;          // ScrollView Content

    private void Start()
    {
        PopulateShop();
    }

    private void PopulateShop()
    {
        foreach (var crop in availableCrops)
        {
            GameObject buttonObj = Instantiate(sellButtonPrefab, contentPanel);
            var ui = buttonObj.GetComponent<SellButtonUI>();
            ui.Setup(crop);
        }
    }
}
