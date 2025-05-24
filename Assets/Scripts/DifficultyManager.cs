using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public float SpawnCoolDownDivider;
    public float SpawnCD;
    public float SpawnCDIncreaseCD;
    public GameManager gm;
    void Start()
    {
        StartCoroutine(SpawnControl());
    }
   
    IEnumerator SpawnControl()
    {
        yield return new WaitForSeconds(SpawnCDIncreaseCD);
        if (gm.GameOn == true)
        {
            SpawnCD = SpawnCD * SpawnCoolDownDivider;
        }
        StartCoroutine(SpawnControl());
    }
}
