using UnityEngine;

public class LicenseSpawner : MonoBehaviour
{
    public GameObject licensePrefab;
    public int licenseCount = 6;
    public float spawnRange = 100f; // size of area to spawn within
    public LayerMask groundLayer;

    void Start()
    {
        SpawnLicenses();
    }

    void SpawnLicenses()
    {
        for (int i = 0; i < licenseCount; i++)
        {
            Vector3 spawnPos = GetRandomPointOnTerrain();
            Instantiate(licensePrefab, spawnPos, Quaternion.identity);
        }
    }

    Vector3 GetRandomPointOnTerrain()
    {
        float x = Random.Range(0, spawnRange);
        float z = Random.Range(0, spawnRange);
        float y = 100f; // start raycast from high up

        Ray ray = new Ray(new Vector3(x, y, z), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 200f, groundLayer))
        {
            return hit.point + Vector3.up * 0.1f; // slightly above ground
        }

        return new Vector3(x, 1, z); // fallback
    }
}

