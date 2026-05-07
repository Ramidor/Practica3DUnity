using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieWalkingState : StateMachineBehaviour
{
    float timer;
    public float walkTime = 10f;
    Transform player;
    NavMeshAgent agent;
    Enemy enemy;

    public float detectionRange = 18f;
    public float patrolSpeed = 2f;

    List<Transform> patrolPoints = new List<Transform>();

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        enemy = animator.GetComponent<Enemy>();
        
        if (agent != null)
        {
            agent.speed = patrolSpeed;
        }

        patrolPoints.Clear();
        GameObject patrolPointsParent = GameObject.FindGameObjectWithTag("Waypoints");
        
        if (patrolPointsParent != null)
        {
            foreach (Transform point in patrolPointsParent.transform)
            {
                patrolPoints.Add(point);
            }
            
            // Pequeña seguridad inicial
            if (agent != null && agent.isOnNavMesh && patrolPoints.Count > 0)
            {
                Vector3 nextPosition = patrolPoints[Random.Range(0, patrolPoints.Count)].position;
                agent.SetDestination(nextPosition);
            }
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemy != null && enemy.zombieChannel != null && enemy.zombieChannel.isPlaying == false)
        {
            enemy.zombieChannel.clip = enemy.zombieWalking;
            enemy.zombieChannel.PlayOneShot(enemy.zombieWalking);
        }

        // --- EL ESCUDO ANTI-CRASHEOS ---
        // Solo comprobamos la distancia si el agente existe, está tocando el suelo y NO está calculando la ruta
        if (agent != null && agent.isOnNavMesh && !agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (patrolPoints.Count > 0)
                {
                    agent.SetDestination(patrolPoints[Random.Range(0, patrolPoints.Count)].position);
                }
            }
        }
        // -------------------------------

        timer += Time.deltaTime;
        if (timer >= walkTime)
        {
            animator.SetBool("isWalking", false);
        }

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(player.position, animator.transform.position);
            if (distanceToPlayer <= detectionRange)
            {
                animator.SetBool("isChasing", true);
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(agent.transform.position);
        }
        
        if (enemy != null && enemy.zombieChannel != null)
        {
            enemy.zombieChannel.Stop();
        }
    }
}