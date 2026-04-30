using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotarObjeto : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidadRotacion = 100f; // Ajusta la velocidad en el Inspector

    void Update()
    {
        // Rota el objeto en el eje Y constantemente en cada frame
        transform.Rotate(0f, velocidadRotacion * Time.deltaTime, 0f);
    }
}