using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio; // ¡Necesario para usar el Audio Mixer!

public class ControladorVolumen : MonoBehaviour
{
    [Header("Referencias")]
    public AudioMixer mezcladorAudio;
    public Slider sliderGeneral;
    public Slider sliderMusica;
    public Slider sliderEfectos;

    void Start()
    {
        // 1. Cargamos los datos guardados (por defecto a 1, el máximo)
        float volGeneral = PlayerPrefs.GetFloat("GeneralGuardado", 1f);
        float volMusica = PlayerPrefs.GetFloat("MusicaGuardada", 1f);
        float volEfectos = PlayerPrefs.GetFloat("EfectosGuardados", 1f);

        // 2. Colocamos los sliders visualmente en su sitio
        sliderGeneral.value = volGeneral;
        sliderMusica.value = volMusica;
        sliderEfectos.value = volEfectos;

        // 3. Aplicamos el sonido real a la mesa de mezclas
        CambiarVolumenGeneral(volGeneral);
        CambiarVolumenMusica(volMusica);
        CambiarVolumenEfectos(volEfectos);

        // 4. Conectamos los sliders para que escuchen nuestros movimientos
        sliderGeneral.onValueChanged.AddListener(CambiarVolumenGeneral);
        sliderMusica.onValueChanged.AddListener(CambiarVolumenMusica);
        sliderEfectos.onValueChanged.AddListener(CambiarVolumenEfectos);
    }

    public void CambiarVolumenGeneral(float valor)
    {
        PlayerPrefs.SetFloat("GeneralGuardado", valor);
        mezcladorAudio.SetFloat("VolGeneral", Mathf.Log10(valor) * 20);
    }

    public void CambiarVolumenMusica(float valor)
    {
        PlayerPrefs.SetFloat("MusicaGuardada", valor);
        mezcladorAudio.SetFloat("VolMusica", Mathf.Log10(valor) * 20);
    }

    public void CambiarVolumenEfectos(float valor)
    {
        PlayerPrefs.SetFloat("EfectosGuardados", valor);
        mezcladorAudio.SetFloat("VolEfectos", Mathf.Log10(valor) * 20);
    }
}