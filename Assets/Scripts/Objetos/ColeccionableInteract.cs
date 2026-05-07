using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColeccionableInteract : MonoBehaviour
{
    [Header("Configuración")]
    public string nombreObjeto; 
    
    // --- LO NUEVO: Nuestro candado ---
    private bool yaRecogido = false;

    [Header("Sonidos")]
    public AudioSource audioSourceColeccionable; // El altavoz
    public AudioClip sonidoCoger; // El clip de sonido

    public void Recoger()
    {
        // 1. EL CANDADO: Si ya lo hemos cogido, cancelamos todo inmediatamente y salimos de la función.
        if (yaRecogido) return;
        
        // Si no lo habíamos cogido, cerramos el candado para siempre.
        yaRecogido = true;

        // 2. HACERLO DESAPARECER VISUALMENTE
        // Apagamos la malla (MeshRenderer) para que se vuelva invisible
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        if (mesh != null) mesh.enabled = false;

        // Apagamos las colisiones para que el raycast de interacción no lo detecte más
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        
        // (Si el objeto tiene hijos visuales, tendrías que apagar el GameObject de los hijos aquí)

        // 3. Dar la recompensa
        InventarioManager.instance.AnadirObjeto(nombreObjeto);
        PuntuacionManager.instance.SumarPuntos(1000f);

        // 4. Reproducir el sonido y esperar a la destrucción final
        if (audioSourceColeccionable != null && sonidoCoger != null)
        {
            audioSourceColeccionable.PlayOneShot(sonidoCoger);
            StartCoroutine(EsperarSonidoYDestruir());
        }
        else
        {
            // Por si acaso te olvidas de ponerle el audio, que se destruya igual
            Destroy(gameObject);
        }
    }

    private IEnumerator EsperarSonidoYDestruir()
    {
        // El objeto es invisible e intocable, pero sigue vivo reproduciendo este sonido
        yield return new WaitForSeconds(sonidoCoger.length);
        
        // Se acabó el sonido, matamos los restos invisibles del objeto
        Destroy(gameObject);
    }
}