using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public float SpawnV;
    public float SpawnCD;
    public float SpawnCDICD;
    public GameManager gm;
    void Start()
    {
        StartCoroutine(SpawnControl());
    }
   
    IEnumerator SpawnControl()
    {
        yield return new WaitForSeconds(SpawnCDICD);
        if (gm.GameOn == true)
        {
            SpawnCD = SpawnCD * SpawnV;
        }
        StartCoroutine(SpawnControl());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
