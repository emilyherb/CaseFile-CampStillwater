using UnityEngine;

public class LicensePickup : MonoBehaviour
{
    private LicenseUIManager uiManager;

    void Start()
    {
        uiManager = FindObjectOfType<LicenseUIManager>();
    }

void OnTriggerEnter(Collider other)
{
    Debug.Log("Triggered by: " + other.name);

    if (other.CompareTag("Player"))
    {
        Debug.Log("License collected!");
        FindObjectOfType<LicenseUIManager>()?.AddLicense();
        Destroy(gameObject);
    }
}
}

