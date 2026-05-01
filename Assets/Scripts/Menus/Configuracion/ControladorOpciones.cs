using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorOpciones : MonoBehaviour
{
    // Start is called before the first frame update
   public GameObject panelConfiguracion; // Arrastraremos el panel aquí
   public GameObject panelMain;

    void Start()
    {
        // Por si acaso, nos aseguramos de que el panel de configuración empiece apagado
        if (panelConfiguracion != null)
        {
            panelConfiguracion.SetActive(false);
        }
       // panelMain= GameObject.FindWithTag("PanelMain");
    }
       public void AbrirConfiguracion()
    {
        // Activamos el panel de configuración
        panelConfiguracion.SetActive(true);
        panelMain.SetActive(false);
    }

    public void CerrarConfiguracion()
    {
        // Desactivamos el panel de configuración (para el botón "Volver" o "X" dentro del panel)
        panelConfiguracion.SetActive(false);
        panelMain.SetActive(true);
    }

    
}
