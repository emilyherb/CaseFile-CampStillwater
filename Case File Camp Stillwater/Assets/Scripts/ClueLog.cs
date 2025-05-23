using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueLog : MonoBehaviour
{
    // Start is called before the first frame update
    public static ClueLog Instance;
    private Dictionary<string, string> collectedClues = new Dictionary<string, string>();

    void Start()
    {
        
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddClue(string id, string title, string text)
    {
        if (!collectedClues.ContainsKey(id))
        {
            collectedClues[id] = $"<b>{title}</b>\n{text}";
        }
    }

    public List<string> GetAllClues()
    {
        return new List<string>(collectedClues.Values);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
