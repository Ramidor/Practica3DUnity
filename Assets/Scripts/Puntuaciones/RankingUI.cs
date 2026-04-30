using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;   
using UnityEngine.SceneManagement;

public class RankingUI : MonoBehaviour 
{
    public TMP_InputField nameInput; // Arrastra tu InputField aquí
    public TMP_Text scoreText;       // Texto que muestra "Tu puntuación: XX"
    
    private float currentPoints; // Obtenemos los puntos actuales del manager

    void Start() {
      
        currentPoints = PuntuacionManager.puntos;
        scoreText.text = "Puntos: " + currentPoints.ToString();
    }

    public void OnClickGuardar() {
        string playerName = nameInput.text;

        if (!string.IsNullOrEmpty(playerName)) {
            // 1. Llamamos al script que creamos antes para guardar
            FindObjectOfType<RankingManager>().AddScore(playerName, currentPoints);
            
            // 2. Desactivar el input y el botón para que no guarde dos veces
            nameInput.interactable = false;
            
            // 3. (Opcional) Refrescar la lista visual o volver al menú
            Debug.Log("Guardado con éxito");
        }
    }
}