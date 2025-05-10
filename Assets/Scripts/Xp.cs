using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xp : MonoBehaviour
{
    public GameObject xpCollectiblePrefab;
    public Transform SpawnPoint;

    public float speed = 10f;
    

    // Start is called before the first frame update
    void Start()
    {
    }

    public void dropxpp()
    {
        DropXP();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 offset = new Vector3(0, 1, 0);

    private void DropXP()
    {
       GameObject xp = Instantiate(xpCollectiblePrefab, SpawnPoint.position + offset, SpawnPoint.rotation);
        Rigidbody rb = xp.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = SpawnPoint.forward * speed;
        }
        
    }
}
