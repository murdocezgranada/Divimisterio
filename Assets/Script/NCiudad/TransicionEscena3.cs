using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TransicionEscena3 : MonoBehaviour
{
    public Image pantallaNegra;          // Imagen negra para oscurecer la pantalla
    public Image imagenAnimada;          // Imagen que aparece en la transición
    public string escenaSiguiente;       // Nombre de la escena a cargar

    public AudioSource audioSource;      // Para reproducir sonido
    public AudioClip sonidoTransicion;   // Sonido de la transición
    public AudioSource musicaFondo;      // Música de fondo

    private bool yaTransicionando = false; // Para evitar múltiples llamadas

    void Start()
    {
        // Estado inicial: todo invisible
        if (pantallaNegra != null)
            pantallaNegra.color = new Color(0, 0, 0, 0);

        if (imagenAnimada != null)
        {
            imagenAnimada.color = new Color(1, 1, 1, 0);
            imagenAnimada.transform.localScale = Vector3.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!yaTransicionando && collision.CompareTag("Player"))
        {
            yaTransicionando = true;
            IniciarTransicion();
        }
    }

    public void IniciarTransicion()
    {
        if (musicaFondo && musicaFondo.isPlaying)
            musicaFondo.Stop();

        // Asegurar que la imagen animada esté lista para animarse
        if (imagenAnimada != null)
        {
            imagenAnimada.gameObject.SetActive(true);
            imagenAnimada.color = new Color(1, 1, 1, 0);
            imagenAnimada.transform.localScale = Vector3.zero;
        }

        pantallaNegra.DOFade(1, 1f).OnComplete(() =>
        {
            imagenAnimada.DOFade(1, 0.5f);

            imagenAnimada.transform
                .DOScale(1.5f, 1f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .OnStart(() =>
                {
                    if (sonidoTransicion && audioSource)
                        audioSource.PlayOneShot(sonidoTransicion);
                })
                .OnComplete(() =>
                {
                    SceneManager.LoadScene(escenaSiguiente);
                });
        });
    }
}
