using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject licensePrefab;
    public GameObject batteryPrefab;
    public GameObject flashLight;
    public int totalLicenses = 6;
    public int totalBatteries = 8;
    public TMP_Text licenseCounterText;

    // Audio stuff
    public AudioSource pickupSource;
    public AudioClip pickupIdClip;

    // Define the bounds of the spawn area
    public Vector3 spawnAreaCenter;
    public Vector3 spawnAreaSize;

    private int licensesCollected = 0;
    private List<GameObject> spawnedLicenses = new List<GameObject>();
    private List<GameObject> spawnedBatteries = new List<GameObject>();

    public GameObject endScreen;
    public GameObject player;

        void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        licensesCollected = 0;
        SpawnLicenses();
        UpdateLicenseUI();
        SpawnBatteries();
    }

    public void CollectLicense(GameObject license)
    {
        // play pickup liscence
        pickupSource.PlayOneShot(pickupIdClip);
        licensesCollected++;
        UpdateLicenseUI();
        Destroy(license);

        if (licensesCollected >= totalLicenses)
        {
            Debug.Log("All licenses collected!");
            // You can add a win screen or other behavior here
        }
    }

    void UpdateLicenseUI()
    {
        licenseCounterText.text = "Licenses: " + licensesCollected + " / " + totalLicenses;
    }

public void PlayerDied()
{
    if (endScreen != null)
    {
        endScreen.SetActive(true);
        Debug.Log("End screen should now be active: " + endScreen.activeSelf);
    }
    else
    {
        Debug.LogWarning("End screen GameObject not assigned!");
    }

    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    if (player != null)
    {
        var controller = player.GetComponent<CharacterController>();
        if (controller != null)
            controller.enabled = false;

        var fpsController = player.GetComponent<FirstPersonController>();
        if (fpsController != null)
            fpsController.enabled = false;
    }
}


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void SpawnLicenses()
    {
        for (int i = 0; i < totalLicenses; i++)
        {
            Vector3 randomPos = GetRandomPosition();
            GameObject license = Instantiate(licensePrefab, randomPos, Quaternion.identity);
            spawnedLicenses.Add(license);
        }
    }

    void SpawnBatteries()
    {
        for (int i = 0; i < totalBatteries; i++)
        {
            Vector3 randomPos = GetRandomPosition();
            GameObject battery = Instantiate(batteryPrefab, randomPos, Quaternion.identity);
            spawnedBatteries.Add(battery);
        }
    }

    public void RechargeLight()
    {
        flashlight flashlightScript = flashLight.GetComponent<flashlight>();

        if (flashlightScript != null)
        {
            flashlightScript.RechargeLight();
        }
        else
        {
            Debug.LogWarning("No Flashlight script found on flashLight GameObject!");
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector3 halfSize = spawnAreaSize * 0.5f;
        float x = Random.Range(-halfSize.x, halfSize.x);
        float y = Random.Range(0.5f, 2f); // Slight vertical offset to keep it above ground
        float z = Random.Range(-halfSize.z, halfSize.z);
        return spawnAreaCenter + new Vector3(x, y, z);
    }
    public void ShowEndScreen()
{
    // Show the end screen UI
    if (endScreen != null)
    {
        endScreen.SetActive(true);
    }

    // Disable player movement
    if (player != null)
    {
        var controller = player.GetComponent<CharacterController>();
        if (controller != null)
            controller.enabled = false;

        var fpsController = player.GetComponent<FirstPersonController>();
        if (fpsController != null)
            fpsController.enabled = false;
    }
}

}
