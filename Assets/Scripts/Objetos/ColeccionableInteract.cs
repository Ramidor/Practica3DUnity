using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColeccionableInteract : MonoBehaviour
{
    [Header("Configuración")]
    public string nombreObjeto; 

      [Header("Sonidos")]
    public AudioSource audioSourceColeccionable; // El altavoz
    public AudioClip sonidoCoger; // El clip de sonido para abrir la puerta

    public void Recoger()
    {
        // 1. Añadimos el objeto a la lista de nuestro Manager
        InventarioManager.instance.AnadirObjeto(nombreObjeto);

        // 2. Opcional: Sumar unos puntos de recompensa por encontrarlo
         PuntuacionManager.instance.SumarPuntos(1000f);

        if (audioSourceColeccionable != null && sonidoCoger != null)
        {
            audioSourceColeccionable.PlayOneShot(sonidoCoger);
            StartCoroutine(EsperarSonidoYDestruir());
        }
        else
        {
            // 3. Destruimos el objeto del mapa (o lo desactivamos)
            Destroy(gameObject);
        }
    }

    private IEnumerator EsperarSonidoYDestruir()
    {
        // Espera a que termine el clip antes de destruir el objeto.
        yield return new WaitForSeconds(sonidoCoger.length);
        Destroy(gameObject);
    }
}