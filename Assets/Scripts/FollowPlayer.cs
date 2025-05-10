using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameManager gm;
    public Vector3 offset;
    void Update()
    {
        if(gm.GameOn==true)
        {
            GameObject player = GameObject.Find("Player");
            transform.position = player.transform.position + offset;
        }

    }
}
