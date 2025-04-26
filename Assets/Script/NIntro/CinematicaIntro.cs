using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class CinematicaIntro : MonoBehaviour
{
    [System.Serializable]
    public class Escena
    {
        public Sprite imagen;
        public string dialogo;
        public AudioClip sonido;
        public float duracion = 3f; // Duración total de la escena
        public bool activarTelefono;
        public float tiempoAparicionTelefono = 0f;
        public float duracionTelefono = 1.5f;
        public AudioClip sonidoTelefono;
        public float tiempoExtraLectura = 2f;
    }

    public Image imagenUI;
    public TextMeshProUGUI textoUI;
    public AudioSource audioSource;
    public AudioSource telefonoAudioSource;
    public GameObject telefonoAnimacion;
    public GameObject imagenADesaparecer;
    public Escena[] escenas;
    public string siguienteEscena;
    public float velocidadTexto = 0.05f;
    public GameObject botonSaltar; 
    void Start()
    {
        telefonoAnimacion.SetActive(false);
    if (imagenADesaparecer) imagenADesaparecer.SetActive(true);
    if (botonSaltar) botonSaltar.SetActive(false); // Asegurar que inicie desactivado
    StartCoroutine(MostrarEscenas());
    }

    IEnumerator MostrarEscenas()
    {
        for (int i = 0; i < escenas.Length; i++)
    {
        Escena escena = escenas[i];
        imagenUI.sprite = escena.imagen;
        textoUI.text = "";

        float duracionTexto = escena.dialogo.Length * velocidadTexto;

        if (escena.activarTelefono)
        {
            StartCoroutine(MostrarTelefono(escena.tiempoAparicionTelefono, escena.duracionTelefono, escena.sonidoTelefono));
        }

        if (escena.sonido)
        {
            audioSource.clip = escena.sonido;
            audioSource.loop = true;
            audioSource.Play();
        }

        // Mostrar el texto gradualmente
        yield return StartCoroutine(MostrarTextoGradual(escena.dialogo, velocidadTexto));
        audioSource.Stop();

        // Si es el segundo diálogo, activar el botón (pero no volverlo a desactivar)
        if (i == 1 && botonSaltar)
        {
            botonSaltar.SetActive(true);
        }

        yield return new WaitForSeconds(Mathf.Max(escena.duracion - duracionTexto, 0) + escena.tiempoExtraLectura);
    }

    FindObjectOfType<TransicionEscena>().IniciarTransicion();
    }

public void SaltarAlUltimoDialogo()
{
    if (botonSaltar) botonSaltar.SetActive(false); // Ocultar el botón al saltar
    StopAllCoroutines(); 
    StartCoroutine(MostrarUltimoDialogo());
}

IEnumerator MostrarUltimoDialogo()
{
    Escena ultimaEscena = escenas[escenas.Length - 1];

    imagenUI.sprite = ultimaEscena.imagen;
    textoUI.text = "";

    if (ultimaEscena.sonido)
    {
        audioSource.clip = ultimaEscena.sonido;
        audioSource.loop = true;
        audioSource.Play();
    }

    yield return StartCoroutine(MostrarTextoGradual(ultimaEscena.dialogo, velocidadTexto));
    audioSource.Stop();

    yield return new WaitForSeconds(Mathf.Max(ultimaEscena.duracion - (ultimaEscena.dialogo.Length * velocidadTexto), 0) + ultimaEscena.tiempoExtraLectura);

    FindObjectOfType<TransicionEscena>().IniciarTransicion();
}
    IEnumerator MostrarTextoGradual(string dialogo, float velocidad)
    {
        foreach (char letra in dialogo)
        {
            textoUI.text += letra;
            yield return new WaitForSeconds(velocidad);
        }
    }

    IEnumerator MostrarTelefono(float tiempoInicio, float duracion, AudioClip sonidoTelefono)
    {
        yield return new WaitForSeconds(tiempoInicio);
        telefonoAnimacion.SetActive(true);
        if (imagenADesaparecer) imagenADesaparecer.SetActive(false); // Ocultar la imagen cuando aparece el teléfono

        if (sonidoTelefono && telefonoAudioSource)
        {
            telefonoAudioSource.PlayOneShot(sonidoTelefono);
        }

        yield return new WaitForSeconds(duracion);

        telefonoAnimacion.SetActive(false);
        if (imagenADesaparecer) imagenADesaparecer.SetActive(true); // Volver a mostrar la imagen cuando desaparece el teléfono
    }
}