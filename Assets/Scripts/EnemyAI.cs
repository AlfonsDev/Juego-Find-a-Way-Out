using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Configuración de Seguimiento")]
    public Transform target;
    public float detectionRange = 15f;
    public float speed = 3.5f;
    
    private NavMeshAgent navAgent;
    
    void Start()
    {
        // Obtener el NavMesh Agent
        navAgent = GetComponent<NavMeshAgent>();
        
        if (navAgent != null)
        {
            navAgent.speed = speed;
            navAgent.stoppingDistance = 1f;
        }
        
        // Si no se asignó target manualmente, buscar por tag
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player1");
            if (player != null)
            {
                target = player.transform;
                Debug.Log("Target encontrado: " + target.name);
            }
        }
    }
    
    void Update()
    {
        if (target != null && navAgent != null && navAgent.isOnNavMesh)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            
            // Si está dentro del rango de detección, seguir
            if (distanceToTarget <= detectionRange)
            {
                navAgent.SetDestination(target.position);
            }
        }
    }
}
