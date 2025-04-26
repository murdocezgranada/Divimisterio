using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDeFallo : MonoBehaviour
{
    private ClickControl[] objetosOcultos; // Arreglo de todos los objetos con ClickControl
    private ClickControl referenciaControl; // Referencia a cualquier ClickControl para contar fallos

    void Start()
    {
        // Busca todos los objetos con el script ClickControl en la escena
        objetosOcultos = FindObjectsOfType<ClickControl>();

        // Usa uno de ellos como referencia para los intentos fallidos
        if (objetosOcultos.Length > 0)
        {
            referenciaControl = objetosOcultos[0];
            Debug.Log("Referencia a ClickControl asignada correctamente.");
        }
        else
        {
            Debug.LogError("No se encontraron objetos con el script ClickControl.");
        }
    }

    void OnMouseDown()
    {
        Debug.Log("¡Hiciste clic en el área de fallo!");

        if (referenciaControl != null)
        {
            referenciaControl.ClickFueraDeObjeto(); // Llama al método de fallos
        }
    }
}