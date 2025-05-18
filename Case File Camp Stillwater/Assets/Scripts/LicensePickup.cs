using UnityEngine;

public class LicensePickup : MonoBehaviour
{
    public float rotateSpeed = 50f;
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.1f;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Rotate to sparkle
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

        // Pulsate
        float scale = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = originalScale * scale;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.CollectLicense(this.gameObject);
        }
    }
}


