using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptBrillo : MonoBehaviour
{
    public Slider sliderBrillo;
    public float sliderValue;
    public Image imagenBrillo;

    void Start()
    {
      
        float posicionGuardada = PlayerPrefs.GetFloat("Brillo", 0.5f);
        sliderBrillo.value = posicionGuardada; 
        ActualizarEfectoBrillo(posicionGuardada);
    }

    public void CambiarBrillo(float valor)
    {
        PlayerPrefs.SetFloat("Brillo", valor);
        ActualizarEfectoBrillo(valor);
    }


    private void ActualizarEfectoBrillo(float valor)
    {
        sliderValue = 0.85f - valor;      
        imagenBrillo.color = new Color(
            imagenBrillo.color.r, 
            imagenBrillo.color.g, 
            imagenBrillo.color.b, 
            sliderValue
        );
    }
}