using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ClickControl : MonoBehaviour
{
    public static string nameofobj;
    public GameObject objnametext;
    public Transform objnametextPos;
    public Transform sucessclick;

    public static int miVariableEstatica = 8;
    private int intentosFallidos = 0;
    private const int maxIntentos = 8;

    public GameObject imagenFinal;
    public Button botonContinuar;
    public GameObject areaDeClick;
    public List<GameObject> objetosOcultos;

    public TextMeshProUGUI intentosText;

    private List<string> mensajesDeAliento;
    public TransicionEscena1 transicion; // Referencia a la transición

    public AudioSource audioSource; // Referencia al AudioSource
    public AudioClip sonidoComún; // Sonido a reproducir cuando se hace clic

    void Start()
    {
        if (imagenFinal != null) imagenFinal.SetActive(false);
        if (botonContinuar != null)
        {
            botonContinuar.gameObject.SetActive(false);
            botonContinuar.onClick.RemoveAllListeners();
            botonContinuar.onClick.AddListener(CargarSiguienteEscena);
        }

        if (areaDeClick != null) areaDeClick.SetActive(true);

        mensajesDeAliento = new List<string>
        {
            "¡Ánimo! Tenemos que encontrar el equipo.",
            "¡No te rindas, ya casi lo tienes!",
            "¡Un poco más! Sigue buscando.",
            "¡Última oportunidad, sigue buscando!",
            "¡Vas muy bien! ¡No pares ahora!",
            "¡Cada intento te acerca más a tu objetivo!",
            "¡Lo lograrás, no te detengas!",
            "¡Tu esfuerzo está a punto de dar frutos!"
        };

        transicion = FindObjectOfType<TransicionEscena1>(); // Busca automáticamente el script en la escena
        ActualizarTextoIntentos();
    }

    void OnMouseDown()
    {
        miVariableEstatica--;
        nameofobj = gameObject.name;
        
        // Reproduce el sonido común cuando se hace clic
        ReproducirSonido();

        Destroy(gameObject);
        Destroy(objnametext);
        Instantiate(sucessclick, objnametextPos.position, sucessclick.rotation);

        if (miVariableEstatica <= 0)
        {
            MostrarImagenFinal();
        }
    }

    void ReproducirSonido()
    {
        if (audioSource != null && sonidoComún != null)
        {
            audioSource.PlayOneShot(sonidoComún); // Reproduce el sonido sin interrumpir otros sonidos
        }
    }

    void MostrarImagenFinal()
    {
        if (imagenFinal != null)
        {
            Debug.Log("Mostrando imagen final.");
            imagenFinal.SetActive(true);
        }

        if (botonContinuar != null)
        {
            Debug.Log("Mostrando botón de continuar.");
            botonContinuar.gameObject.SetActive(true);
        }
    }

    public void ClickFueraDeObjeto()
    {
        intentosFallidos++;
        Debug.Log("Intentos fallidos: " + intentosFallidos);
        ActualizarTextoIntentos();
    }

    void ActualizarTextoIntentos()
    {
        string mensajeDeAliento = mensajesDeAliento[Random.Range(0, mensajesDeAliento.Count)];

        if (intentosText != null)
        {
            intentosText.text = "Intentos fallidos: " + intentosFallidos + "/" + maxIntentos + "\n" + mensajeDeAliento;
        }
    }

    void CargarSiguienteEscena()
    {
        if (botonContinuar != null)
        {
            Debug.Log("Ocultando botón de continuar...");
            botonContinuar.gameObject.SetActive(false); // Oculta el botón
            botonContinuar.transform.parent.gameObject.SetActive(false); // Oculta el contenedor si es necesario
        }

        if (transicion != null)
        {
            Debug.Log("Iniciando transición...");
            transicion.IniciarTransicion();
        }
        else
        {
            Debug.LogError("No se encontró el script de transición en la escena.");
        }
    }
}
