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
    public Animator anim;
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameObject.name == "FastEnemy(Clone)")
        {
            anim.speed = 2f; // Set animation speed to 2x
        }
        else if (gameObject.name == "StrongEnemy(Clone)")
        {
            anim.speed = 0.75f; // Set animation speed to 0.75x
        }
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
