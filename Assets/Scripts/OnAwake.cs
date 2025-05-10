using System.Collections.Generic;
using UnityEngine;

public class OnAwake : MonoBehaviour
{
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
    }



    void Awake()
    {
        rb.AddForce(this.transform.forward * 50, ForceMode.Impulse);
        Destroy(gameObject, 0.2f);
    }
}
