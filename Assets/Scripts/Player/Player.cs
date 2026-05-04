using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int HP = 100;
    
    public void RecibirDaño(int daño)
    {
        HP -= daño;
        if (HP <= 0)
        {
          print("Has muerto");

        }
        else
        {
            print("Has recibido " + daño + " puntos de daño. HP restante: " + HP);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            RecibirDaño(other.gameObject.GetComponent<ZombieHand>().daño); 
        }
    }
}
