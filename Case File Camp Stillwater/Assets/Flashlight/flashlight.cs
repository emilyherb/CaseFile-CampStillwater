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

    public AudioSource clickSource;
    public AudioClip flashlightClick;
    public AudioClip changeBattery;

    public UpdatedMonster monster;

    void Start()
    {
        totalBars = batteryBars.Length;
        flashlightLight.enabled = false;
        flashlightCollider.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (batteryLife > 0f)
            {
                // switch flashlight on/off
                isFlashlightOn = !isFlashlightOn;
                flashlightLight.enabled = isFlashlightOn;
                flashlightCollider.enabled = isFlashlightOn;
                clickSource.PlayOneShot(flashlightClick);
            }
        }
        if (batteryLife > 0f)
        {
            monster.needsToKillPlayer = false;
        }
        if (isFlashlightOn && batteryLife > 0f)
        {
            //Debug.Log(batteryLife);
            batteryLife -= drainRate * Time.deltaTime;
            batteryLife = Mathf.Clamp(batteryLife, 0f, 100f);

            if (batteryLife <= 0f)
            {
                monster.needsToKillPlayer = true;
                isFlashlightOn = false;
                flashlightLight.enabled = false;
                flashlightCollider.enabled = false;
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

    public void RechargeLight()
    {
        batteryLife = 100f;
        clickSource.PlayOneShot(changeBattery);
        UpdateBatteryUI();
    }
}
