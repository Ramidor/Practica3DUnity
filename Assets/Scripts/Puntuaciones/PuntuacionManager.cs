using UnityEngine;
using TMPro; // Necesario para poder modificar la interfaz

public class PuntuacionManager : MonoBehaviour
{
    public static PuntuacionManager instance;

    [Header("Sistema de Puntos")]
    public float puntos = 10000f; // Los puntos REALES que tienes
    private float puntosMostrados; // Los puntos VISUALES que se están animando

    [Header("Interfaz y Animación")]
    public TextMeshProUGUI textoPuntosUI; // Arrastra aquí tu Texto_Puntos
    public float velocidadAnimacion = 1000f; // Puntos que bajan/suben por segundo

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Al empezar la partida, igualamos los puntos visuales a los reales para que no haya animación inicial
        puntosMostrados = puntos;
        ActualizarTexto();
    }

    void Update()
    {
        // Si los puntos que vemos en pantalla son distintos a los reales...
        if (puntosMostrados != puntos)
        {
            // Movemos los puntos mostrados hacia los reales poco a poco
            puntosMostrados = Mathf.MoveTowards(puntosMostrados, puntos, velocidadAnimacion * Time.deltaTime);
            ActualizarTexto();
        }
    }

    public void SumarPuntos(float cantidad)
    {
        puntos += cantidad;
        // No actualizamos el texto aquí, se encarga el Update() de hacerlo visualmente
    }

    public bool GastarPuntos(float cantidad)
    {
        if (puntos >= cantidad)
        {
            puntos -= cantidad;
            return true;
        }
        else
        {
            return false;
        }
    }

    void ActualizarTexto()
    {
        // Asegurarnos de que el texto esté enlazado antes de cambiarlo
        if (textoPuntosUI != null)
        {
            // Usamos Mathf.RoundToInt para que en pantalla no salgan decimales feos (ej: 999.5)
            textoPuntosUI.text = "PUNTOS: " + Mathf.RoundToInt(puntosMostrados).ToString();
        }
    }
}