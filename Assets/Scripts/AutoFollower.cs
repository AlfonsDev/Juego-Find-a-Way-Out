using UnityEngine;

public class AutoFollower : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float detectionRange = 100f;
    [SerializeField] private float stopDistance = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    
    private Transform target;
    private CharacterController controller;
    private Animator animator;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
            controller.center = new Vector3(0, 1, 0);
            controller.radius = 0.5f;
            controller.height = 2f;
        }
        
        animator = GetComponent<Animator>();
        FindTarget();
    }
    
    void FindTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
            Debug.Log("✅ Target encontrado: " + player.name);
        }
    }
    
    void Update()
    {
        if (target != null && controller != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            
            if (distance <= detectionRange && distance > stopDistance)
            {
                // Movimiento
                Vector3 direction = (target.position - transform.position).normalized;
                
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
                
                Vector3 movement = direction * speed * Time.deltaTime;
                movement.y = -9.81f * Time.deltaTime;
                controller.Move(movement);
                
                // USAR IsWalking (con I mayúscula)
                if (animator != null)
                {
                    animator.SetBool("IsWalking", true);
                    Debug.Log("🚶 Activando animación walk");
                }
            }
            else
            {
                // DETENER ANIMACIÓN
                if (animator != null)
                {
                    animator.SetBool("IsWalking", false);
                    Debug.Log("🛑 Activando animación idle");
                }
            }
        }
    }
}