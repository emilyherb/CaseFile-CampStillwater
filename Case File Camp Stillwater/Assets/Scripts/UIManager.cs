using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Panels")]
    public GameObject clueLogPanel; // Assign your clue log panel here in inspector

    private bool isClueLogOpen = false;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ToggleClueLog()
    {
        isClueLogOpen = !isClueLogOpen;
        clueLogPanel.SetActive(isClueLogOpen);

        // Optional: pause the game while the log is open
        Time.timeScale = isClueLogOpen ? 0 : 1;
    }

    // Optional: show a quick popup when a clue is found
    public void ShowClueNotification(string clueTitle)
    {
        // Implement your popup UI here if you want
        Debug.Log($"Clue found: {clueTitle}");
    }
}

