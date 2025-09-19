using UnityEngine;

public class PlantSelectionUI : MonoBehaviour
{
    public static PlantSelectionUI Instance;

    [SerializeField] private Canvas rootCanvas;
    [SerializeField] private GameObject panelPrefab;       
    [SerializeField] private SeedButtonUI seedButtonPrefab; 
    [SerializeField] private SeedData[] availableSeeds;    

    private GameObject currentPanel;
    private Plot currentPlot;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Show(Plot plot)
    {
        Hide(); // clear old panel

        currentPlot = plot;

        // Spawn panel
        currentPanel = Instantiate(panelPrefab, rootCanvas.transform);
        currentPanel.transform.position = plot.transform.position + new Vector3(1.5f, 1.5f, 0f);

        Transform content = currentPanel.transform.Find("Content");
        if (content == null)
        {
            Debug.LogError("PlantPanel prefab must have a child named 'Content'!");
            return;
        }

        // Create a button for each seed
        foreach (var seed in availableSeeds)
        {
            SeedButtonUI btn = Instantiate(seedButtonPrefab, content);
            btn.Setup(seed, currentPlot);
        }
    }

    public void Hide()
    {
        if (currentPanel != null)
        {
            Destroy(currentPanel);
            currentPanel = null;
        }
    }
}
