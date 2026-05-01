using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
public class ScriptPantallaCompleta : MonoBehaviour
{
    public Toggle togglePantallaCompleta;
    public TMP_Dropdown dropdownResolucion;
    Resolution[] resolucionesDisponibles;
    // Start is called before the first frame update
    void Start()
    {
        if(Screen.fullScreen)
        {
            togglePantallaCompleta.isOn = true;
        }
        else
        {
           togglePantallaCompleta.isOn = false;
        }
        RevisarResolucion();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CambiarPantallaCompleta(bool valor)
    {
        Screen.fullScreen = valor;
    }

    public void RevisarResolucion()
    {
        resolucionesDisponibles = Screen.resolutions;
        dropdownResolucion.ClearOptions();
        List<string> opciones = new List<string>();
        int resolucionActualIndex = 0;
        for (int i = 0; i < resolucionesDisponibles.Length; i++)
        {
            string opcion = resolucionesDisponibles[i].width + " x " + resolucionesDisponibles[i].height;
            opciones.Add(opcion);
            if (Screen.fullScreen && resolucionesDisponibles[i].width == Screen.currentResolution.width 
            && resolucionesDisponibles[i].height == Screen.currentResolution.height)
            {
                resolucionActualIndex = i;
            }
        }
        dropdownResolucion.AddOptions(opciones);
        dropdownResolucion.value = resolucionActualIndex;
        dropdownResolucion.RefreshShownValue();
        
        dropdownResolucion.value = PlayerPrefs.GetInt("Resolucion", 0);
    }
    public void CambiarResolucion(int index)
    {
        PlayerPrefs.SetInt("Resolucion", dropdownResolucion.value);
        Resolution resolucionSeleccionada = resolucionesDisponibles[index];
        Screen.SetResolution(resolucionSeleccionada.width, resolucionSeleccionada.height, Screen.fullScreen);
    }
}
