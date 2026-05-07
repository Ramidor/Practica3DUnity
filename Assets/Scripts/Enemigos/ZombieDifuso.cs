using UnityEngine;
using UnityEngine.AI;

public class ZombieDifuso : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;

    [Header("Ataque")]
    public float attackingDistance = 2.5f;

    [Header("Cerebro Difuso (Fuzzy Logic)")]
    [Tooltip("Eje X: Distancia al jugador. Eje Y: Velocidad del NavMeshAgent.")]
    public AnimationCurve velocidadFuzzy;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null || agent == null) return; // Seguridad extra por si muere el jugador

        // 1. Calculamos la distancia real
        float distanceToPlayer = Vector3.Distance(player.position, animator.transform.position);

        // --- EL ESCUDO ANTI-CRASHEOS ---
        // Solo cambiamos la velocidad y le decimos a dónde ir si está pisando el suelo (NavMesh)
        if (agent.isOnNavMesh)
        {
            // 2. LÓGICA DIFUSA: La curva decide la velocidad
            agent.speed = velocidadFuzzy.Evaluate(distanceToPlayer);
            
            // 3. Persecución
            agent.SetDestination(player.position);
        }
        // -------------------------------

        // Rotación suave para que mire al jugador
        Vector3 direction = player.position - animator.transform.position;
        direction.y = 0; 
        if (direction != Vector3.zero) 
        {
            animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 6f);
        }

        // Transición directa a atacar si te alcanza
        if (distanceToPlayer <= attackingDistance)
        {
            animator.SetBool("isAttacking", true);
        }
    }
}