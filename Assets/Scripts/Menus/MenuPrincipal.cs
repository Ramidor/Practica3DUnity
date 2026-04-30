using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escenas

public class MenuPrincipal : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelConfiguracion; // Arrastraremos el panel aquí

    void Start()
    {
        // Por si acaso, nos aseguramos de que el panel de configuración empiece apagado
        if (panelConfiguracion != null)
        {
            panelConfiguracion.SetActive(false);
        }
    }

    // --- MÉTODOS PARA LOS BOTONES ---

    public void Jugar()
    {
        // Cambia "EscenaJuego" por el nombre exacto de tu nivel (respetando mayúsculas)
        SceneManager.LoadScene("Mapa Principal"); 
    }

    public void AbrirRanking()
    {
        // Cambia esto al nombre de tu escena de ranking
        SceneManager.LoadScene("PantallaRecords"); 
    }

    public void AbrirConfiguracion()
    {
        // Activamos el panel de configuración
        panelConfiguracion.SetActive(true);
    }

    public void CerrarConfiguracion()
    {
        // Desactivamos el panel de configuración (para el botón "Volver" o "X" dentro del panel)
        panelConfiguracion.SetActive(false);
    }

    public void SalirDelJuego()
    {
        // Esto cierra el juego cuando está exportado (.exe)
        Application.Quit();
        
        // El Application.Quit no hace nada dentro del editor de Unity, 
        // así que ponemos este log para saber que el botón funciona cuando hacemos pruebas.
        Debug.Log("¡Saliendo del juego!"); 
    }
}