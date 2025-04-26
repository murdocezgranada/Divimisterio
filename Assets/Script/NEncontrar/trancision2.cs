using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TransicionEscena1 : MonoBehaviour
{
    public Image pantallaNegra; // Imagen negra para oscurecer la pantalla
    public Image imagenAnimada; // Imagen que aparece en la transición
    public string escenaSiguiente; // Nombre de la escena a cargar

    public AudioSource audioSource; // AudioSource para reproducir el sonido
    public AudioClip sonidoTransicion; // Sonido de la transición
    public AudioSource musicaFondo; // El AudioSource de la música de fondo (Canvas)
    
    public GameObject botonContinuar; // Referencia al botón de continuar

    void Start()
    {
        pantallaNegra.color = new Color(0, 0, 0, 0);
        imagenAnimada.transform.localScale = Vector3.zero;
        imagenAnimada.color = new Color(1, 1, 1, 0);
    }

    public void IniciarTransicion()
    {
        // Ocultamos el botón antes de empezar la transición
        if (botonContinuar != null)
        {
            Debug.Log(" Ocultando botón de continuar...");
            botonContinuar.SetActive(false);
        }
        else
        {
            Debug.LogError(" botonContinuar no está asignado en el Inspector.");
        }

        if (musicaFondo && musicaFondo.isPlaying)
        {
            musicaFondo.Stop();
        }

        Debug.Log(" Iniciando transición...");

        pantallaNegra.DOFade(1, 1f).OnComplete(() =>
        {
            imagenAnimada.DOFade(1, 0.5f);
            imagenAnimada.transform.DOScale(1.5f, 1f).SetLoops(2, LoopType.Yoyo).OnStart(() =>
            {
                if (sonidoTransicion && audioSource)
                {
                    audioSource.PlayOneShot(sonidoTransicion);
                }
            }).OnComplete(() =>
            {
                SceneManager.LoadScene(escenaSiguiente);
            });
        });
    }
}
