using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
   public ZombieHand manoZombie; // Referencia a la mano del zombie para detectar colisiones

    public int daño; // Cantidad de daño que el zombie inflige al jugador

public void Start()
    {
        // Aseguramos que la mano del zombie tenga el daño correcto
        if (manoZombie != null)
        {
            manoZombie.daño = daño;
        }
    }
}
