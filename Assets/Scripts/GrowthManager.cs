using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GrowthManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    public PlantData plantData;       // assign in Inspector
    public Image plantImage;          // UI Image for sprite (or SpriteRenderer if 2D world)

    private bool isGrowing = false;
    private float growthTimer = 0f;
    private int currentPhase = 0;
    private Plot parentPlot;


    private void Start()
    {
        plantImage.sprite = plantData.growthSprites[0];
        parentPlot = GetComponentInParent<Plot>(); 
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
            growthTimer += Time.deltaTime;

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
            Debug.Log($"{plantData.plantName} harvested!");
            // TODO: Add coins to player here
            ItemManager.Instance.AddItem(plantData.plantName, 1);

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
