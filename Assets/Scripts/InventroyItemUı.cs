using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text countText;

    public void SetData(Sprite sprite, int count)
    {
        if (icon != null) icon.sprite = sprite;
        if (countText != null) countText.text = count.ToString();
    }
}
