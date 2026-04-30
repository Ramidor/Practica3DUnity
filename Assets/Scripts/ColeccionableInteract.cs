using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColeccionableInteract : MonoBehaviour
{
    [Header("Configuración")]
    public string nombreObjeto; 

    public void Recoger()
    {
        // 1. Añadimos el objeto a la lista de nuestro Manager
        InventarioManager.instance.AnadirObjeto(nombreObjeto);

        // 2. Opcional: Sumar unos puntos de recompensa por encontrarlo
         PuntuacionManager.instance.SumarPuntos(1000f);

        // 3. Destruimos el objeto del mapa (o lo desactivamos)
        Destroy(gameObject);
    }
}