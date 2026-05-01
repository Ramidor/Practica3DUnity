using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScriptVolumen : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public float sliderValue;
    public Image imagenMute;
    void Start()
    {
        slider.value =PlayerPrefs.GetFloat("Volumen", 0.5f);     
        sliderValue = slider.value;
        AudioListener.volume = slider.value;
        RevisarMute();
        
    }
    public void CambiarVolumen(float valor)
    {
        sliderValue = valor;
        PlayerPrefs.SetFloat("Volumen", sliderValue);
        AudioListener.volume = sliderValue; 
        RevisarMute();
    }
    public void RevisarMute()
    {
        if (sliderValue == 0)
        {
            imagenMute.enabled = true;
        }
        else
        {
            imagenMute.enabled = false;
        }
    }
}
