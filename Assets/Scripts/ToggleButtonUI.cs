using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ToggleButton : MonoBehaviour
{
    private bool isOn = false;
    private Button btn;
    private Image img;

    public Color onColor = Color.green;
    public Color offColor = Color.red;

    // Event so other scripts (like AutomationUpgradeUI) can listen
    public UnityEvent<bool> onValueChanged = new UnityEvent<bool>();

    void Awake()
    {
        btn = GetComponent<Button>();
        img = GetComponent<Image>();
        btn.onClick.AddListener(Toggle);
        UpdateVisual();
    }

    private void Toggle()
    {
        isOn = !isOn;
        UpdateVisual();
        onValueChanged.Invoke(isOn); // notify listeners
    }

    private void UpdateVisual()
    {
        if (img != null)
            img.color = isOn ? onColor : offColor;
    }

    public bool IsOn() => isOn;

    public void SetState(bool value)
    {
        if (isOn == value) return; // no need to change
        isOn = value;
        UpdateVisual();
    }
}
