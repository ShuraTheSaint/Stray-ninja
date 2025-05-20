using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZombies : MonoBehaviour
{
    Transform spawnPoint;
    public GameObject zombiePrefab;
    public GameManager gm;
    public DifficultyManager spawnInterwal;
    bool start = true;
    public float spawnInterval;
    Vector3 xoffset = new Vector3(30, 0, 0);
    Vector3 yoffset = new Vector3(0, 0, 30);
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnObjectPeriodically());
    }

    void spawnPOINT()
    {
        spawnPoint = GameObject.Find("Player").GetComponent<Transform>();
        start = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(gm.GameOn)
        {
            if(start == true )
            {
                spawnPOINT();
            }
        }
        spawnInterval = spawnInterwal.SpawnCD;
    }

    IEnumerator SpawnObjectPeriodically()
    {
        if (gm.GameOn == true)
        {
            var pickRandom = Random.Range(1, 5);

            if (pickRandom == 1)
            {
                Instantiate(zombiePrefab, spawnPoint.position + xoffset, spawnPoint.rotation);
            }
            if (pickRandom == 2)
            {
                Instantiate(zombiePrefab, spawnPoint.position + xoffset*-1, spawnPoint.rotation);
            }
            if (pickRandom == 3)
            {
                Instantiate(zombiePrefab, spawnPoint.position + yoffset, spawnPoint.rotation);
            }
            if (pickRandom == 4)
            {
                Instantiate(zombiePrefab, spawnPoint.position + yoffset * -1, spawnPoint.rotation);
            }
        }
        yield return new WaitForSeconds(spawnInterval);
        StartCoroutine(SpawnObjectPeriodically());
    }
}
