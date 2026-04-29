using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    public float distanciaInteraccion = 3f; // Distancia máxima para abrir la puerta
    public Camera camaraJugador; // Arrastra tu cámara aquí en el Inspector
    
    [Header("Sistema de Puntos")]
    public float points = 10000f; // Provisional. Luego lo enlazarás a tu otro script.

    void Update()
    {
        // Comprobamos si el jugador pulsa la tecla E
        if (Input.GetKeyDown(KeyCode.E))
        {
            IntentarInteractuar();
        }
    }

    void IntentarInteractuar()
    {
        // Trazamos el Raycast desde el centro de la pantalla
        Ray ray = camaraJugador.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        // Si el raycast choca con algo en el rango establecido...
        if (Physics.Raycast(ray, out hit, distanciaInteraccion))
        {
            // Comprobamos si el objeto con el que chocamos tiene el tag "Puerta"
            if (hit.collider.CompareTag("Puerta"))
            {
                // Buscamos el script de la puerta
                PuertaInteractuable scriptPuerta = hit.collider.GetComponent<PuertaInteractuable>();

                if (scriptPuerta != null)
                {
                    // 1. Comprobamos si la puerta ESTÁ CERRADA todavía
                    if (!scriptPuerta.estaAbierta)
                    {
                        // 2. Comprobamos si tenemos puntos suficientes
                        if (points >= scriptPuerta.costePuerta)
                        {
                            // Restamos los puntos
                            points -= scriptPuerta.costePuerta;
                            Debug.Log("¡Puerta comprada! Puntos restantes: " + points);

                            // Abrimos la puerta
                            scriptPuerta.AlternarPuerta();
                        }
                        else
                        {
                            // No hay puntos suficientes
                            Debug.Log("No tienes suficientes puntos. Necesitas: " + scriptPuerta.costePuerta);
                        }
                    }
                }
            }
        }
    }
}