using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Panels")]
    public GameObject clueLogPanel;
    public GameObject clueLogBook;
    public TextMeshProUGUI clueNotificationText;
    public float notificationDuration = 3f;
    private Coroutine notificationRoutine;

    public bool isClueLogOpen = false;

    void Start()
    {

    }

    void Update()
    {
       if(Input.GetKeyDown(KeyCode.L))
        {
            ToggleClueLog();
        } 
    }

    public bool IsClueLogOpen()
    {
        return isClueLogOpen;
    }


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ToggleClueLog()
    {
        isClueLogOpen = !isClueLogOpen;
        clueLogBook.SetActive(isClueLogOpen);

        // Optional: pause the game while the log is open
        Time.timeScale = isClueLogOpen ? 0 : 1;

        if (isClueLogOpen)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void ShowClueNotification(string clueTitle)
    {
        // Implement your popup UI here if you want
        Debug.Log($"Clue found: {clueTitle}");
        if (notificationRoutine != null)
            StopCoroutine(notificationRoutine);

        string fullMessage = $"{clueTitle} found. Press L to see in log.";
        notificationRoutine = StartCoroutine(ShowClueRoutine(fullMessage));
    }

    private IEnumerator ShowClueRoutine(string message)
    {
        clueNotificationText.text = message;
        clueNotificationText.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(notificationDuration);

        clueNotificationText.gameObject.SetActive(false);
    }

}

