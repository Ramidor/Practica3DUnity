using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // ¡VITAL! Nos permite usar los comandos de navegación

public class ZombieNavMesh : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador; // Aquí arrastraremos a tu jugador

    private NavMeshAgent agente;

    void Start()
    {
        // El zombie busca su propio cerebro al nacer
        agente = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // En cada frame, le decimos al cerebro: "¡Ve a las coordenadas actuales del jugador!"
        if (jugador != null)
        {
            agente.SetDestination(jugador.position);
        }
    }
}