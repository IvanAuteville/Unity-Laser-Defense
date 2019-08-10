using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    private TextMeshProUGUI healthDisplay = null;

    private void Awake()
    {
        healthDisplay = GetComponent<TextMeshProUGUI>();
    }

    public void SetHealth(string health)
    {
        healthDisplay.SetText(health);
    }
}