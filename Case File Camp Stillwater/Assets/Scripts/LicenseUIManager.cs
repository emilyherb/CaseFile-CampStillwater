using TMPro;
using UnityEngine;

public class LicenseUIManager : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    private int collected = 0;
    private int total = 6;

    public void AddLicense()
    {
        collected++;
        UpdateUI();
    }

    void Start()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        counterText.text = $"Licenses Collected: {collected}/{total}";
    }
}

