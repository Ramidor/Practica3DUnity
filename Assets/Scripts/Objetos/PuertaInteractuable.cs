using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuertaInteractuable : MonoBehaviour
{
    [Header("Configuración de la Puerta")]
    public float costePuerta; // Puedes cambiar esto en el Inspector para cada puerta
    public bool estaAbierta = false; // Ahora es pública para que el jugador la lea
    
    private Animator anim;

    [Header("Sonidos")]
    public AudioSource audioSourceJugador; // El altavoz
    public AudioClip sonidoAbrir; // El clip de sonido para abrir la puerta
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void AlternarPuerta()
    {
        // En los juegos de zombies por oleadas, las puertas normalmente 
        // se abren y se quedan abiertas para siempre.
        estaAbierta = true; 
        
        // Le pasamos el valor al parámetro del Animator
        anim.SetBool("estaAbierta", estaAbierta);
        BoxCollider colliderInteraccion = GetComponent<BoxCollider>();
        if (colliderInteraccion != null)
        {
            colliderInteraccion.enabled = false;
        }
    
            if (audioSourceJugador != null && sonidoAbrir != null)
            {
                audioSourceJugador.PlayOneShot(sonidoAbrir);
            }
    }
    public void TerminarPartida()
    {
        StartCoroutine(CargarPantallaConDelay());
    }

    private IEnumerator CargarPantallaConDelay()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("PantallaFinal");
    }
}