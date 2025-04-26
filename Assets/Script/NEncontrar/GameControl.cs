using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject imagenFinal; // Imagen del mensaje final
    public Button botonContinuar; // Botón para continuar

    void Start()
    {
        // Asegurarnos de que la imagen y el botón estén desactivados al principio
        if (imagenFinal != null)
        {
            imagenFinal.SetActive(false);
        }

        if (botonContinuar != null)
        {
            botonContinuar.gameObject.SetActive(false);
            botonContinuar.onClick.AddListener(CargarSiguienteEscena); // Asigna la función correctamente
        }
    }

    // Mostrar la imagen y el botón cuando el jugador termine
    public void MostrarImagenFinal()
    {
        if (imagenFinal != null)
        {
            imagenFinal.SetActive(true);
        }

        if (botonContinuar != null)
        {
            botonContinuar.gameObject.SetActive(true);
            botonContinuar.interactable = true; // Asegurarnos de que el botón sea interactivo
        }
    }

    void CargarSiguienteEscena()
    {
        Debug.Log("Botón Continuar presionado. Cargando siguiente escena...");
        SceneManager.LoadScene("TuEscenaSiguiente"); // Reemplazar con el nombre exacto de la escena
    }
}
