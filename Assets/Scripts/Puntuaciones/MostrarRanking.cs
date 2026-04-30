using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Para manipular los textos

public class MostrarRanking : MonoBehaviour
{
    [Header("Referencias")]
    public RankingManager rankingManager; // El script que lee el JSON
    public GameObject filaPrefab;         // Tu molde de la fila
    public Transform contenedor;          // El panel con el Vertical Layout Group

    void Start()
    {
        // Si el RankingManager no carga los datos en el Awake o están en otra escena,
        // asegúrate de que el JSON esté leído antes de llamar a esta función.
        ConstruirTabla();
    }

    public void ConstruirTabla()
    {
        // 1. Limpiamos el contenedor por si había filas viejas
        foreach (Transform hijo in contenedor)
        {
            Destroy(hijo.gameObject);
        }

        // 2. Comprobamos si hay datos guardados
        if (rankingManager.ranking == null || rankingManager.ranking.scores.Count == 0)
        {
            Debug.Log("El ranking está vacío.");
            return;
        }

        // 3. Generamos una fila por cada jugador guardado en el JSON
        for (int i = 0; i < rankingManager.ranking.scores.Count; i++)
        {
            // Creamos una copia del Prefab dentro del contenedor
            GameObject nuevaFila = Instantiate(filaPrefab, contenedor);

            // Obtenemos todos los textos de esa fila (el del nombre y el de los puntos)
            TextMeshProUGUI[] textos = nuevaFila.GetComponentsInChildren<TextMeshProUGUI>();

            if (textos.Length >= 2)
            {
                // Modificamos el primer texto (Posición y Nombre)
                textos[0].text = (i + 1) + ". " + rankingManager.ranking.scores[i].playerName;
                
                // Modificamos el segundo texto (Puntos)
                textos[1].text = rankingManager.ranking.scores[i].score.ToString();
            }
        }
    }
}