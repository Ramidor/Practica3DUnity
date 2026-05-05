using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;
    private NavMeshAgent navAgent;

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

            Destroy(gameObject, 4f); // Destruye el objeto después de 4 segundos para que la animación de muerte se reproduzca
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
        Gizmos.DrawWireSphere(transform.position, 18f); // Start chasing
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 21f); // Stop chasing

    }
}
