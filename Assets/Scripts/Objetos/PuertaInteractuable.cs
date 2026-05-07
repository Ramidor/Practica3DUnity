using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuertaInteractuable : MonoBehaviour
{
    [Header("Configuración de la Puerta")]
    public float costePuerta; 
    public bool estaAbierta = false; 
    
    private Animator anim;

    [Header("Sonidos")]
    public AudioSource audioSourceJugador; 
    public AudioClip sonidoAbrir; 

    // --- LO NUEVO ---
    [Header("Zonas de Spawn (Call of Duty)")]
    [Tooltip("Arrastra aquí los puntos de spawn de la nueva habitación que esconde esta puerta")]
    public List<Transform> nuevosSpawnsDesbloqueados;
    
    // El cerebro central que controla las oleadas
    private ZombieSpawnerController spawnerPrincipal;
    // ----------------

    void Start()
    {
        anim = GetComponent<Animator>();

        // Buscamos el Spawner automáticamente en el mapa para no tener que arrastrarlo a mano
        spawnerPrincipal = FindObjectOfType<ZombieSpawnerController>();
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

        // --- LO NUEVO: ¡La conexión! ---
        // Si la puerta tiene spawns guardados, se los inyectamos al Spawner al abrirse
        if (spawnerPrincipal != null && nuevosSpawnsDesbloqueados.Count > 0)
        {
            spawnerPrincipal.UnlockNewSpawns(nuevosSpawnsDesbloqueados);
        }
        // -------------------------------
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