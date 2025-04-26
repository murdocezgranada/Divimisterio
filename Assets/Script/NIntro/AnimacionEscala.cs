using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TransicionEscena : MonoBehaviour
{
    public Image pantallaNegra; // Imagen negra para oscurecer la pantalla
    public Image imagenAnimada; // Imagen que aparece en la transición
    public string escenaSiguiente; // Nombre de la escena a cargar

    public AudioSource audioSource; // AudioSource para reproducir el sonido
    public AudioClip sonidoTransicion; // Sonido de la transición
    public AudioSource musicaFondo; // El AudioSource de la música de fondo (Canvas)

    void Start()
    {
        // Asegurar que la pantalla negra y la imagen están invisibles al inicio
        pantallaNegra.color = new Color(0, 0, 0, 0);
        imagenAnimada.transform.localScale = Vector3.zero;
        imagenAnimada.color = new Color(1, 1, 1, 0);
    }

    public void IniciarTransicion()
    {
        // Detener completamente la música de fondo si está activa
        if (musicaFondo && musicaFondo.isPlaying)
        {
            musicaFondo.Stop(); // Detener la música por completo
        }

        // Oscurecer la pantalla
        pantallaNegra.DOFade(1, 1f).OnComplete(() =>
        {
            // Una vez que la pantalla negra esté completamente opaca,
            // aparece la imagen animada y el sonido de la transición se reproduce
            imagenAnimada.DOFade(1, 0.5f);
            imagenAnimada.transform.DOScale(1.5f, 1f).SetLoops(2, LoopType.Yoyo).OnStart(() =>
            {
                // Reproducir sonido de transición cuando la imagen animada empieza a aparecer
                if (sonidoTransicion && audioSource)
                {
                    audioSource.PlayOneShot(sonidoTransicion);
                }
            }).OnComplete(() =>
            {
                // Cargar la nueva escena después de la animación
                SceneManager.LoadScene(escenaSiguiente);
            });
        });
    }
}