using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    public float distanciaInteraccion = 3f;
    public Camera camaraJugador;

    [Header("Interfaz de Usuario (UI)")]
    public TextMeshProUGUI textoInteraccion;

    [Header("Sonidos")]
    public AudioSource audioSourceJugador; // El altavoz
    public AudioClip sonidoErrorPuntos; // El clip de sonido para el error de puntos

    void Update()
    {
        textoInteraccion.text = "";

        Ray ray = camaraJugador.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanciaInteraccion))
        {
            // ===== COMPROBAMOS SI ES UNA PUERTA =====
            if (hit.collider.CompareTag("Puerta"))
            {
                PuertaInteractuable scriptPuerta = hit.collider.GetComponent<PuertaInteractuable>();

                if (scriptPuerta != null && !scriptPuerta.estaAbierta)
                {
                    textoInteraccion.text = "Pulsar [E] para abrir puerta [" + scriptPuerta.costePuerta + "]";

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        IntentarComprar(scriptPuerta);
                    }
                }
            }
            // ===== COMPROBAMOS SI ES UN COLECCIONABLE =====
            else if (hit.collider.CompareTag("Coleccionable"))
            {
                ColeccionableInteract scriptCol = hit.collider.GetComponent<ColeccionableInteract>();

                if (scriptCol != null)
                {
                    textoInteraccion.text = "Pulsar [E] para recoger: " + scriptCol.nombreObjeto;

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        scriptCol.Recoger();
                        textoInteraccion.text = ""; // Limpiamos el texto al recogerlo
                    }
                }
            }
            else if (hit.collider.CompareTag("PuertaFinal"))
            {
                PuertaInteractuable scriptPuerta = hit.collider.GetComponent<PuertaInteractuable>();

                if (scriptPuerta != null && !scriptPuerta.estaAbierta)
                {
                    textoInteraccion.text = "Pulsar [E] para abrir puerta [" + scriptPuerta.costePuerta + "] y terminar partida";

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        IntentarComprarPuertaFinal(scriptPuerta);
                    }
                }
            }
        }
    }

    void IntentarComprar(PuertaInteractuable scriptPuerta)
    {
        // Aquí ocurre la magia del Singleton. Llamamos a PuntuacionManager.instance
        // Si GastarPuntos devuelve true, significa que teníamos el dinero y ya nos lo ha cobrado.
        if (PuntuacionManager.instance.GastarPuntos(scriptPuerta.costePuerta))
        {
            // Borramos el texto y abrimos la puerta
            textoInteraccion.text = "";
            scriptPuerta.AlternarPuerta();
        }
        else
        {
            Debug.Log("No tienes suficientes puntos.");
            if (audioSourceJugador != null && sonidoErrorPuntos != null)
            {
                audioSourceJugador.PlayOneShot(sonidoErrorPuntos);
            }
        }
    }

    void IntentarComprarPuertaFinal(PuertaInteractuable scriptPuerta)
    {
        // Aquí ocurre la magia del Singleton. Llamamos a PuntuacionManager.instance
        // Si GastarPuntos devuelve true, significa que teníamos el dinero y ya nos lo ha cobrado.
        if (PuntuacionManager.instance.GastarPuntos(scriptPuerta.costePuerta))
        {
            // Borramos el texto y abrimos la puerta
            textoInteraccion.text = "Salida desbloqueada. ¡Enhorabuena por terminar la partida!";
            PlayerPrefs.SetFloat("PuntosFinales", PuntuacionManager.instance.puntos);
            scriptPuerta.TerminarPartida();
        }
        else
        {
            Debug.Log("No tienes suficientes puntos.");
            if (audioSourceJugador != null && sonidoErrorPuntos != null)
            {
                audioSourceJugador.PlayOneShot(sonidoErrorPuntos);
            }
            
        }
    }
}