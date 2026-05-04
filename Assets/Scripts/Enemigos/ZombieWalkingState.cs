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

    public float detectionRange = 18f;
    public float patrolSpeed = 2f;

    List<Transform> patrolPoints = new List<Transform>();

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;

        patrolPoints.Clear();

        GameObject patrolPointsParent = GameObject.FindGameObjectWithTag("Waypoints");
        foreach (Transform point in patrolPointsParent.transform)
        {
            patrolPoints.Add(point);
        }
        Vector3 nextPosition = patrolPoints[Random.Range(0, patrolPoints.Count)].position;
        agent.SetDestination(nextPosition);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(patrolPoints[Random.Range(0, patrolPoints.Count)].position);
        }

        timer += Time.deltaTime;
        if (timer >= walkTime)
        {
            animator.SetBool("isWalking", false);
        }

        float distanceToPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceToPlayer <= detectionRange)
        {
            animator.SetBool("isChasing", true);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);


    }
}
