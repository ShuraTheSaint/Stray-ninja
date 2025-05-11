using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemycontroller : MonoBehaviour
{
    public float speed;
    Transform target;
    public NavMeshAgent agent;
    GameManager gm;
    GameObject player;
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        player = GameObject.Find("Player");
        target = player.transform;

        if (gm.GameOn == true)
        {
            agent.speed = speed;
            float distance = Vector3.Distance(target.position, transform.position);

            agent.SetDestination(target.position);
        }
    }
}
