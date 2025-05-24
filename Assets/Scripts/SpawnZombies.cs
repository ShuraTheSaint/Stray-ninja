using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZombies : MonoBehaviour
{
    Transform spawnPoint;
    public GameObject zombiePrefab;
    public GameObject StrongerZombiePrefab;
    public GameObject FasterZombiePrefab;
    public GameManager gm;
    public DifficultyManager spawnInterwal;
    float spawnInterval;
    bool gameBegun = false;


    void Update()
    {
        if (gm.GameOn == true)
        {
            if (gameBegun == false)
            {
                spawnPoint = GameObject.Find("Player").GetComponent<Transform>();
                StartCoroutine(SpawnObjectPeriodically());
                gameBegun = true;
            }
            spawnInterval = spawnInterwal.SpawnCD;
        }
    }

    IEnumerator SpawnObjectPeriodically()
    {
        if (gm.GameOn == true)
        {
            // Random angle and distance for spawn position
            float angle = Random.Range(0f, 360f);
            float radius = 40f; // Use the same as xoffset/yoffset magnitude
            Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;
            Vector3 spawnPos = spawnPoint.position + offset;

            // Randomly pick enemy type: 0=normal, 1=strong, 2=fast
            int enemyType = Random.Range(0, 10);
            GameObject prefabToSpawn = zombiePrefab;
            if (enemyType == 1)
                prefabToSpawn = StrongerZombiePrefab;
            else if (enemyType == 2)
                prefabToSpawn = FasterZombiePrefab;

            // Make zombie face the player
            Vector3 directionToPlayer = (spawnPoint.position - spawnPos).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);

            Instantiate(prefabToSpawn, spawnPos, lookRotation);
        }
        yield return new WaitForSeconds(spawnInterval);
        StartCoroutine(SpawnObjectPeriodically());
    }
}
