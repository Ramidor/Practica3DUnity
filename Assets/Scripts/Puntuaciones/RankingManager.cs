using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

public class RankingManager : MonoBehaviour
{
    [Header("UI Pantalla Guardar")]
    public TMP_InputField inputNombre;
    public TextMeshProUGUI textoPuntosFinales;
    public TextMeshProUGUI textoColeccionablesFinales;
    
    [Header("UI Pantalla Records")]
    public TextMeshProUGUI textoUiRanking;

    
    private int puntosParaGuardar;
    public int coleccionablesRecogidos;
    private string rutaArchivo;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None; // Quita el bloqueo del centro
        Cursor.visible = true;                  // Vuelve a hacer visible la flechita
        rutaArchivo = Application.persistentDataPath + "/ranking.json";

        // 1. Recuperamos los puntos de PlayerPrefs (el 0 es por si no encuentra nada)
        float puntosRecuperados = PlayerPrefs.GetFloat("PuntosFinales", 0f);
        puntosParaGuardar = Mathf.FloorToInt(puntosRecuperados);
        coleccionablesRecogidos = PlayerPrefs.GetInt("ColeccionablesTotales", 0);

        // 2. Si estamos en la pantalla de meter el nombre, actualizamos el texto
        if (textoPuntosFinales != null)
        {
            textoPuntosFinales.text = puntosParaGuardar.ToString();
        }

        if (textoColeccionablesFinales != null)
        {
            textoColeccionablesFinales.text = "Has recogido "+ coleccionablesRecogidos.ToString()+" coleccionables. Si conseguiste 4, tuviste un bono de 10000 puntos!";
        }

        // 3. Si estamos en la pantalla de los records, mostramos la lista
        if (SceneManager.GetActiveScene().name == "PantallaRecords")
        {
            MostrarRanking();
        }
    }

    public void GuardarEnRanking()
    {
        string nombreJugador = inputNombre.text;
        if (string.IsNullOrEmpty(nombreJugador)) nombreJugador = "Anonimo";

        RankingData miRanking = new RankingData();
        
        // Leer archivo existente si lo hay
        if (File.Exists(rutaArchivo))
        {
            string json = File.ReadAllText(rutaArchivo);
            miRanking = JsonUtility.FromJson<RankingData>(json);
        }

        // Añadir nueva puntuación
        miRanking.scores.Add(new PlayerScore { nombre = nombreJugador, puntos = puntosParaGuardar });
        
        // Ordenar de mayor a menor y quedarse con los 10 mejores
        miRanking.scores = miRanking.scores.OrderByDescending(x => x.puntos).Take(8).ToList();

        // Guardar el JSON actualizado
        string nuevoJson = JsonUtility.ToJson(miRanking);
        File.WriteAllText(rutaArchivo, nuevoJson);

        // Cambiar a la escena de records
        SceneManager.LoadScene("PantallaRecords");
    }

    public void MostrarRanking()
    {
        if (File.Exists(rutaArchivo))
        {
            string json = File.ReadAllText(rutaArchivo);
            RankingData datos = JsonUtility.FromJson<RankingData>(json);

            string textoRanking = "TOP 8 RANKING:\n\n";
            int posicion = 1;
            
            foreach (var entrada in datos.scores)
            {
                // He añadido el número de posición para que quede más chulo
                textoRanking += $"{posicion}. {entrada.nombre}: {entrada.puntos}\n";
                posicion++;
            }
            
            if(textoUiRanking != null)
            {
                textoUiRanking.text = textoRanking;
            }
        }
        else
        {
            if(textoUiRanking != null) textoUiRanking.text = "Aún no hay récords.";
        }
    }

    public void VolverAlMenu()
    {
        StartCoroutine(LoadRankingSceneAfterDelay());
    }

    private IEnumerator LoadRankingSceneAfterDelay()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("MenuPrincipal");
    }
}