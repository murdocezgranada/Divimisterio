using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TransicionEscena6 : MonoBehaviour
{
    public Image pantallaNegra;          // Imagen negra para oscurecer la pantalla
    public Image imagenAnimada;          // Imagen que aparece en la transición
    public string escenaSiguiente;       // Nombre de la escena a cargar

    public AudioSource audioSource;      // Para reproducir sonido
    public AudioClip sonidoTransicion;   // Sonido de la transición
    public AudioSource musicaFondo;      // Música de fondo

    public GameObject finalPanel;        // El panel de felicitaciones que queremos ocultar

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

    public void IniciarTransicion()
    {
        // Aseguramos que no se inicie una nueva transición si ya está en proceso
        if (yaTransicionando) return;
        yaTransicionando = true;

        // Ocultar el panel final (si está visible)
        if (finalPanel != null)
            finalPanel.SetActive(false);

        // Detener música de fondo si está sonando
        if (musicaFondo && musicaFondo.isPlaying)
            musicaFondo.Stop();

        // Asegurar que la imagen animada esté lista para animarse
        if (imagenAnimada != null)
        {
            imagenAnimada.gameObject.SetActive(true);
            imagenAnimada.color = new Color(1, 1, 1, 0);
            imagenAnimada.transform.localScale = Vector3.zero;
        }

        // Comienza la animación
        pantallaNegra.DOFade(1, 1f).OnComplete(() =>
        {
            imagenAnimada.DOFade(1, 0.5f);

            imagenAnimada.transform
                .DOScale(1.5f, 1f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .OnStart(() =>
                {
                    // Reproducir el sonido de la transición
                    if (sonidoTransicion && audioSource)
                        audioSource.PlayOneShot(sonidoTransicion);
                })
                .OnComplete(() =>
                {
                    // Debug para asegurarnos de que está cambiando de escena
                    Debug.Log("Animación completada, cambiando de escena...");
                    SceneManager.LoadScene(escenaSiguiente);  // Cargar la siguiente escena
                });
        });
    }
}
