using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // ¡MUY IMPORTANTE! Nos permite modificar la UI

public class InventarioManager : MonoBehaviour
{
    public static InventarioManager instance;

    [Header("Inventario")]
    public List<string> listaColeccionables = new List<string>();

    [Header("Interfaz de Usuario")]
    public TextMeshProUGUI textoGemasUI; // El hueco para arrastrar tu texto

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Al empezar la partida, forzamos a que el texto ponga "0"
        ActualizarTextoUI();
    }

    public void AnadirObjeto(string nombreObjeto)
    {
        // 1. Metemos la gema en la lista interna
        listaColeccionables.Add(nombreObjeto);
        Debug.Log("¡" + nombreObjeto + " recogido! Tienes " + listaColeccionables.Count + " coleccionables.");

        // 2. Actualizamos la pantalla para que el jugador lo vea
        ActualizarTextoUI();
    }

    void ActualizarTextoUI()
    {
        // Si hemos arrastrado el texto en Unity, lo cambiamos
        if (textoGemasUI != null)
        {
            // .Count cuenta cuántos objetos hay dentro de la lista
            textoGemasUI.text = ": " + listaColeccionables.Count; 
        }
    }
}