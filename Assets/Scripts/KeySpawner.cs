using Unity.VisualScripting;
using UnityEngine;

public class KeySpawner : MonoBehaviour
{
    public GameObject [] keyPrefabs; // Reference to the key prefab
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 spawnPos;
        int maxAttempts = 100;
        Vector3 lastpos = Vector3.zero;
        bool validSpawn;
        for (int i = 0; i < keyPrefabs.Length; i++)
        {
            spawnPos = Vector3.zero;
            validSpawn = false;
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                // Random angle and distance for spawn position
                float angle = Random.Range(0f, 360f);
                float radius = Random.Range(50f, 100f);
                Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;
                spawnPos = gameObject.transform.position + offset + Vector3.up;

                // Check for obstacles at spawn position (adjust radius as needed)
                float checkRadius = 3.0f;
                Collider[] hitColliders = Physics.OverlapSphere(spawnPos, checkRadius, LayerMask.GetMask("Obstacles"));
                if (hitColliders.Length == 0 && Vector3.Distance(lastpos, spawnPos)>50)
                {
                    validSpawn = true;
                    break;
                }
            }

            if (validSpawn)
            {
                lastpos = spawnPos;
                Instantiate(keyPrefabs[i], spawnPos, Quaternion.identity);
            }
        }
    }
}
