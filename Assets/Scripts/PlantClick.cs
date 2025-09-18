using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlantClick : MonoBehaviour
{
    public GrowthManager growthManager;
    public float clickBoost = 1f; // seconds reduced per click

    private void OnMouseDown()   // works for world objects
    {

        if (growthManager.IsFullyGrown())
        {
            growthManager.Harvest();
        }
        else
        {
            growthManager.SpeedUp(clickBoost);
            Debug.Log("Clicked!");
        }
    }

}


