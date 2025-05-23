using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CluePickup : MonoBehaviour
{
    // Start is called before the first frame update
    public string clueID;
    public string clueTitle;
    [TextArea]
    public string clueText;

    private bool collected = false;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (collected) return;

        Debug.Log("Something entered trigger: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player triggered pickup!");
            collected = true;
            ClueLog.Instance.AddClue(clueID, clueTitle, clueText);
            //UIManager.Instance.ShowClueNotification(clueTitle);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
