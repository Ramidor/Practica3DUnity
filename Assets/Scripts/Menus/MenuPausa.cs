using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelPausa;

    private bool juegoPausado = false;

    void Start()
    {
        // Nos aseguramos de que los menús empiecen apagados
        panelPausa.SetActive(false);
    }

    void Update()
    {
        // Detectamos si el jugador pulsa Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Si el panel de configuración está abierto, Escape lo cierra y vuelve a la pausa base
            if (panelPausa.activeSelf)
            {
                 if (juegoPausado) Reanudar();
                
            }else Pausar();
           
        }
    }

    public void Pausar()
    {
        juegoPausado = true;
        Time.timeScale = 0f; // ¡ESTO CONGELA EL JUEGO (los zombies, balas, físicas...)!
        panelPausa.SetActive(true);

        // ===== DESBLOQUEAR EL RATÓN =====
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Reanudar()
    {
        juegoPausado = false;
        Time.timeScale = 1f; // El tiempo vuelve a la normalidad (1x velocidad)
        panelPausa.SetActive(false);
     

        // ===== VOLVER A BLOQUEAR EL RATÓN PARA EL FPS =====
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SalirAlMenu()
    {
        // ¡SUPER IMPORTANTE! Devolver el tiempo a la normalidad antes de cambiar de escena
        // Si no lo haces, ¡el menú principal también se quedará congelado!
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MenuPrincipal"); // Pon el nombre exacto de tu menú
    }
}