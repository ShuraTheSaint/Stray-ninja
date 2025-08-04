using UnityEngine;
using UnityEngine.AI;

public class Enemycontroller : MonoBehaviour
{
    public float speed = 3.5f;
    public float updateThreshold = 1f; // Minimum movement before updating path
    public float maxDistanceFromTarget = 100f; // Customize as needed

    private Transform target;
    private NavMeshAgent agent;
    private GameManager gm;
    private GameObject player;
    private Vector3 lastTargetPosition;
    private SpawnZombies zombieCount; // Assign in Inspector

    void Start()
    {
        zombieCount = GameObject.Find("Spawner").GetComponent<SpawnZombies>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        target = player.transform;
        lastTargetPosition = target.position;
    }

    void Update()
    {
        if (gm.GameOn)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (!agent.isOnNavMesh || distanceToTarget > maxDistanceFromTarget)
            {
                zombieCount.zombieCount--;
                Destroy(gameObject);
                return;
            }

            agent.speed = speed;

            float distanceMoved = Vector3.Distance(target.position, lastTargetPosition);
            if (distanceMoved > updateThreshold)
            {
                agent.SetDestination(target.position);
                lastTargetPosition = target.position;
            }
        }
    }
}