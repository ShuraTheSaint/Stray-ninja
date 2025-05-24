using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xp : MonoBehaviour
{
    public GameObject xpCollectiblePrefab;
    public Transform SpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void dropxpp(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            float radius = 0.5f; // Adjust for how far apart you want the drops
            Vector2 randomCircle = Random.insideUnitCircle * radius;
            Vector3 randomOffset;

            if (i > 0)
            {
                randomOffset = new Vector3(randomCircle.x, 1, randomCircle.y);
            }
            else
            {
                randomOffset = new Vector3(0, 1, 0);
            }

            GameObject xp = Instantiate(xpCollectiblePrefab, SpawnPoint.position + randomOffset, SpawnPoint.rotation);
        }
    }
}
