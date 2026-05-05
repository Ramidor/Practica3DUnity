using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAttackState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent agent;
    Enemy enemy;
      
        public float stopAttackingDistance = 3f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        enemy = animator.GetComponent<Enemy>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
     

        if (enemy != null && enemy.zombieChannel != null && enemy.zombieChannel.isPlaying == false)
        {
            enemy.zombieChannel.clip = enemy.zombieAttack;
            enemy.zombieChannel.PlayOneShot(enemy.zombieAttack);
        }
        LookAtPlayer();
         float distanceToPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceToPlayer >= stopAttackingDistance)
        {
            animator.SetBool("isAttacking", false);
        }


    }

    private void LookAtPlayer()
    {
       Vector3 direction = player.position - agent.transform.position;
       agent.transform.rotation = Quaternion.LookRotation(direction);

       var yRotation = agent.transform.rotation.eulerAngles.y;
         agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.zombieChannel.Stop();
    }
}
