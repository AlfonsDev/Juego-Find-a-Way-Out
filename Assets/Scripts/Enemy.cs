using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float followDistance = 15f;
    
    private NavMeshAgent agent;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player1");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }
    
    void Update()
    {
        if (player != null && agent != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            
            if (distance <= followDistance)
            {
                agent.SetDestination(player.position);
            }
        }
    }
}