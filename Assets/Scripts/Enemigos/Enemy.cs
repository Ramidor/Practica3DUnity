using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] public int HP = 100;
    private Animator animator;
    private NavMeshAgent navAgent;
    public bool isDead = false;

    public AudioClip zombieWalking;
    public AudioClip zombieDeath;
    public AudioClip zombieAttack;
    public AudioClip zombieHurt;
    public AudioClip zombieChase;

    public AudioSource zombieChannel;
    public AudioSource zombieChannel2;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void RecibirDaño(int daño)
    {
        HP -= daño;
        if (HP <= 0)
        {
            int randomValue = Random.Range(0, 2);
            if (randomValue == 0)
            {
                animator.SetTrigger("Morir1");
            }
            else
            {
                animator.SetTrigger("Morir2");
            }
            zombieChannel.PlayOneShot(zombieDeath);
             GetComponent<Collider>().enabled = false; // Desactiva el collider para evitar más colisiones
            isDead = true;

            PuntuacionManager.instance.SumarPuntos(100f);

           
        }
        else
        {
            animator.SetTrigger("RecibirDaño");
            zombieChannel2.PlayOneShot(zombieHurt);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f); // Ataque
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 200f); // Start chasing
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 201f); // Stop chasing
    }

}
