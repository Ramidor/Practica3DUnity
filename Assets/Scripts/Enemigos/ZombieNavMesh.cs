using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // ¡VITAL! Nos permite usar los comandos de navegación

public class ZombieNavMesh : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador; // Aquí arrastraremos a tu jugador

    private NavMeshAgent agente;
    private Enemy enemy;

    void Start()
    {
        // El zombie busca su propio cerebro al nacer
        agente = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        // Si el zombie está muerto, no le damos más órdenes de movimiento
        if (enemy != null && enemy.isDead)
        {
            return;
        }

        // En cada frame, le decimos al cerebro: "¡Ve a las coordenadas actuales del jugador!"
        if (jugador != null && agente.isOnNavMesh)
        {
            agente.SetDestination(jugador.position);
        }
    }
}