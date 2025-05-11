using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkedOn : MonoBehaviour
{
    public string [] Tags;
    Experience expS;

    private void Start()
    {
        expS = GameObject.Find("Player").GetComponent<Experience>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags[0]))
        {
            expS.AddExperience(1);
            Destroy(gameObject);
        }
    }
}
