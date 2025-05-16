using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class flashlight : MonoBehaviour
{
    public Image[] batteryBars;
    public float drainRate;
    public Light flashlightLight;
    public Collider flashlightCollider;

    private float batteryLife = 100f;
    private bool isFlashlightOn = false;
    private int totalBars;

    void Start()
    {
        totalBars = batteryBars.Length;
        flashlightLight.enabled = false;
        flashlightCollider.enabled = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            Debug.Log("flashlight clicked");
            if (batteryLife > 0f)
            {
                // switch flashlight on/off
                isFlashlightOn = !isFlashlightOn;
                flashlightLight.enabled = isFlashlightOn;
                flashlightCollider.enabled = isFlashlightOn;
            }
        }
        if (isFlashlightOn && batteryLife > 0f)
        {
            Debug.Log(batteryLife);
            batteryLife -= drainRate * Time.deltaTime;
            batteryLife = Mathf.Clamp(batteryLife, 0f, 100f);

            if (batteryLife <= 0f)
            {
                isFlashlightOn = false;
                flashlightLight.enabled = false;
                Debug.Log("Flashlight ran out of battery");
            }

            UpdateBatteryUI();
        }
    }

    void UpdateBatteryUI()
    {
        int barsToShow = Mathf.CeilToInt((batteryLife / 100f) * totalBars);

        for (int i = 0; i < totalBars; i++)
        {
            batteryBars[i].enabled = i < barsToShow;
        }
    }
}
