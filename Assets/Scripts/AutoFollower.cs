using UnityEngine;

public class AutoFollower : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float detectionRange = 100f;
    [SerializeField] private float stopDistance = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    
    // ✅ NUEVOS PARÁMETROS PARA ATAQUE
    [Header("Configuración de Ataque")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 2f;

    [Header("Sonidos")]
[SerializeField] private AudioClip idleSound;
[SerializeField] private AudioClip walkSound;
[SerializeField] private AudioClip attackSound;

private AudioSource audioSource;
private float lastSoundTime;
    
    private Transform target;
    private CharacterController controller;
    private Animator animator;
    
    // ✅ NUEVAS VARIABLES PARA ATAQUE
    private float lastAttackTime;
    private bool isAttacking;
    
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

            audioSource = GetComponent<AudioSource>();
    if (audioSource == null)
        audioSource = gameObject.AddComponent<AudioSource>();
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
            
            if (distance <= detectionRange)
            {
                // ✅ VERIFICAR SI ESTÁ EN RANGO DE ATAQUE
                if (distance <= attackRange)
                {
                    HandleAttack();
                }
                else if (distance > stopDistance)
                {
                    HandleMovement();
                }
                else
                {
                    HandleIdle();
                }
            }
            else
            {
                HandleIdle();
            }
        }
    }
    
    // ✅ NUEVA FUNCIÓN: Manejar movimiento (código existente)
    void HandleMovement()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        Vector3 movement = direction * speed * Time.deltaTime;
        movement.y = -9.81f * Time.deltaTime;
        controller.Move(movement);
        
        // Mantener animaciones existentes
        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsAttacking", false);
            Debug.Log("🚶 Activando animación walk");
        }
        
        isAttacking = false;

        PlaySoundWithDelay(walkSound, 0.5f);
    }
    
    // ✅ NUEVA FUNCIÓN: Manejar estado idle
    void HandleIdle()
    {
        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsAttacking", false);
            Debug.Log("🛑 Activando animación idle");
        }
        
        isAttacking = false;
        PlaySoundWithDelay(idleSound, 3f);
    }
    
    // ✅ NUEVA FUNCIÓN: Manejar ataque
    void HandleAttack()
    {
        // Rotar hacia el jugador
        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        // Verificar cooldown y ejecutar ataque
        if (Time.time >= lastAttackTime + attackCooldown && !isAttacking)
        {
            StartAttack();
        }
        
        // Actualizar animador
        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsAttacking", isAttacking);
            Debug.Log("⚔️ Activando animación attack");
        }
    }
    
    // ✅ NUEVA FUNCIÓN: Iniciar ataque
    void StartAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        Debug.Log("⚔️ Iniciando ataque!");
        audioSource.PlayOneShot(attackSound);
    }
    
    // ✅ NUEVA FUNCIÓN: Llamada por Animation Event
    public void OnAttackComplete()
    {
        isAttacking = false;
        Debug.Log("✅ Ataque completado");
    }
    
    // ✅ NUEVA FUNCIÓN: Visualización en el editor
    void OnDrawGizmosSelected()
    {
        // Rango de detección (amarillo)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Distancia de parada (azul)
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
        
        // Rango de ataque (rojo)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void PlaySoundWithDelay(AudioClip clip, float delay)
{
    if (clip != null && Time.time >= lastSoundTime + delay)
    {
        audioSource.PlayOneShot(clip);
        lastSoundTime = Time.time;
    }
}
} 