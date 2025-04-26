using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class DialogoN4 : MonoBehaviour
{
    [System.Serializable]
    public class Escena
    {
        public Sprite imagen;
        public string dialogo;
        public AudioClip sonido;
        public float duracion = 3f;
        public float tiempoExtraLectura = 2f;
    }

    public Image imagenUI;
    public TextMeshProUGUI textoUI;
    public AudioSource audioSource;
    public Escena[] escenas;
    public string siguienteEscena;
    public float velocidadTexto = 0.05f;
    public GameObject botonSaltar; 

    void Start()
    {
        if (botonSaltar) botonSaltar.SetActive(false);
        StartCoroutine(MostrarEscenas());
    }

    IEnumerator MostrarEscenas()
    {
        for (int i = 0; i < escenas.Length; i++)
        {
            Escena escena = escenas[i];
            imagenUI.sprite = escena.imagen;
            textoUI.text = "";

            if (escena.sonido)
            {
                audioSource.clip = escena.sonido;
                audioSource.loop = true;
                audioSource.Play();
            }

            yield return StartCoroutine(MostrarTextoGradual(escena.dialogo, velocidadTexto));
            audioSource.Stop();

            if (i == 1 && botonSaltar)
            {
                botonSaltar.SetActive(true);
            }

            yield return new WaitForSeconds(Mathf.Max(escena.duracion - (escena.dialogo.Length * velocidadTexto), 0) + escena.tiempoExtraLectura);
        }
        SceneManager.LoadScene("MNivel1");
    }

    public void SaltarAlUltimoDialogo()
    {
        if (botonSaltar) botonSaltar.SetActive(false);
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

        SceneManager.LoadScene("MNivel1");
    }

    IEnumerator MostrarTextoGradual(string dialogo, float velocidad)
    {
        foreach (char letra in dialogo)
        {
            textoUI.text += letra;
            yield return new WaitForSeconds(velocidad);
        }
    }
}
