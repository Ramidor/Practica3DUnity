using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escenas

public class MenuPrincipal : MonoBehaviour
{
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

    public void SalirDelJuego()
    {
        // Esto cierra el juego cuando está exportado (.exe)
        Application.Quit();
        
        // El Application.Quit no hace nada dentro del editor de Unity, 
        // así que ponemos este log para saber que el botón funciona cuando hacemos pruebas.
        Debug.Log("¡Saliendo del juego!"); 
    }
}