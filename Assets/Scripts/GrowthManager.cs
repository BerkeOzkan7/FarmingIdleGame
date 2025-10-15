using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GrowthManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    public PlantData plantData;       
    public Image plantImage;          

    private bool isGrowing = false;
    private float growthTimer = 0f;
    private int currentPhase = 0;
    private Plot parentPlot;
    public SeedData seedData;


    private void Start()
    {
        plantImage.sprite = plantData.growthSprites[0];
        parentPlot = GetComponentInParent<Plot>(); 
    }

    public void Init(SeedData seed)
    {
        seedData = seed;
    }
    void Update()
    {
        if (IsFullyGrown()) return;


        float phaseDuration = plantData.totalGrowthTime / (plantData.growthSprites.Length - 1);


        int newPhase = Mathf.Min((int)(growthTimer / phaseDuration), plantData.growthSprites.Length - 1);
    
        if(newPhase != currentPhase)
        {
            currentPhase = newPhase;
            plantImage.sprite = plantData.growthSprites[currentPhase];
        }

        if (isGrowing)
        {
            float speedBonus = UpgradeManager.Instance.GetBonusValue(seedData, UpgradeType.Speed);
            growthTimer += Time.deltaTime * (1 + speedBonus);

            float remaining = Mathf.Max(0, plantData.totalGrowthTime - growthTimer);

            // Update UI
            if (timerText != null)
            {
                timerText.text = Mathf.CeilToInt(remaining).ToString() + "s";
            }

            if (growthTimer >= plantData.totalGrowthTime)
            {
                FinishGrowth();
            }
        }
    }

    private void FinishGrowth()
    {
        isGrowing = false;
        Debug.Log("Plant fully grown!");
        if (timerText != null)
            timerText.text = "Ready!";
    }

    public void StartGrowth()
    {
        growthTimer = 0f;
        isGrowing = true;
    }

    public void SpeedUp(float seconds)
    {
        if (!IsFullyGrown())
        {
            growthTimer += seconds;
        }
    }

    public bool IsFullyGrown()
    {
        return currentPhase == plantData.growthSprites.Length - 1;
    }

    public void Harvest()
    {
        if (IsFullyGrown())
        {
            int baseAmount = 1;

            // Safe check in case seedData is null
            int bonus = (seedData != null) ? Mathf.RoundToInt(UpgradeManager.Instance.GetBonusValue(seedData, UpgradeType.Yield)) : 0;
            int total = baseAmount + bonus;

            Debug.Log($"{seedData.seedName} harvested! Yield = {total}");

            // Add to inventory using SeedData item name
            ItemManager.Instance.AddItem(seedData.cropName, total);

            parentPlot.ClearPlot();
            Destroy(gameObject);
        }
    }
}

[System.Serializable]

public class PlantData
{
    public string plantName; 
    public float totalGrowthTime;
    public float sellPrice;
    public Sprite[] growthSprites;
}
