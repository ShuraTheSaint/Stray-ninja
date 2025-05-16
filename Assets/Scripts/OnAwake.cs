using System.Collections.Generic;
using UnityEngine;

public class OnAwake : MonoBehaviour
{
    public Rigidbody rb;
    private static Upgrades up; // Static cache

    void Awake()
    {
        if (up == null) up = GameObject.Find("Upgrade Manager")?.GetComponent<Upgrades>();
        rb.AddForce(this.transform.forward * 50, ForceMode.Impulse);
        if(up.smoothThrow)
        {
            Destroy(gameObject, 0.4f);
        }
        else Destroy(gameObject, 0.2f);
    }
}

