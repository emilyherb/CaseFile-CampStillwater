using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ClueLog : MonoBehaviour
{
    // Start is called before the first frame update
    public static ClueLog Instance;
    private Dictionary<string, Clue> collectedClues = new Dictionary<string, Clue>();

    public GameObject clueButtonPrefab; // UI Button prefab with TMP_Text
    public Transform clueListParent;    // UI container for clue list (left page)
    public Text clueTitleText;   // UI for right page title
    public Text clueContentText;

    [System.Serializable]
    public class Clue
    {
        public string id;
        public string title;
        public string content;
    }

    void Start()
    {
        
    }

    void Awake()
    {
        Debug.Log("ClueLog Awake called!");

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddClue(string id, string title, string text)
    {
        Debug.Log($"AddClue called with ID: {id}");

        if (collectedClues.ContainsKey(id))
        {
            Debug.Log("Clue already collected. Skipping.");
            return;
        }

        Clue newClue = new Clue { id = id, title = title, content = text };
        collectedClues[id] = newClue;

        GameObject buttonObj = Instantiate(clueButtonPrefab, clueListParent);
        Debug.Log("Created clue button: " + buttonObj.name);

        buttonObj.GetComponentInChildren<Text>().text = title;
        buttonObj.GetComponent<Button>().onClick.AddListener(() => DisplayClue(newClue));
    }


    public void DisplayClue(Clue clue)
    {
        Debug.Log($"Displaying clue: {clue.title}");
        clueTitleText.text = clue.title;
        clueContentText.text = clue.content;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
