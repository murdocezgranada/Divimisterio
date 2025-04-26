using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MovimientoJugador : MonoBehaviour
{
    private Rigidbody2D rb2D;

    [Header("Movimiento")]
    private float movimientoHorizontal = 0f;
    [SerializeField] private float velocidadDeMovimiento = 5f;
    [Range(0, 0.3f)][SerializeField] private float suavizadoDeMovimiento = 0.05f;
    private Vector3 velocidad = Vector3.zero;
    private bool mirandoDerecha = true;

    [Header("Animación")]
    private Animator animator;

    [Header("Interacción")]
    public Image imagenDialogo;
    public TMP_Text textoDialogo;
    public AudioSource audioSource;
    public float velocidadEscritura = 0.05f;
    public GameObject botonSaltar;

    [System.Serializable]
    public class EscenaDialogo
    {
        public Sprite imagen;
        public string dialogo;
        public AudioClip sonido;
        public float duracion = 5f;
        public float tiempoExtraLectura = 5f;
    }

    public List<EscenaDialogo> escenasDialogo;
    private bool enDialogo = false;
    private Coroutine dialogoCoroutine;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (imagenDialogo != null)
            imagenDialogo.gameObject.SetActive(false);
        
        if (botonSaltar != null)
            botonSaltar.SetActive(false);
    }

    private void Update()
    {
        if (!enDialogo)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                {
                    movimientoHorizontal = (touch.position.x < Screen.width / 2) ? -velocidadDeMovimiento : velocidadDeMovimiento;
                }
            }
            else
            {
                movimientoHorizontal = 0;
            }

            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                movimientoHorizontal = Input.GetAxisRaw("Horizontal") * velocidadDeMovimiento;
            }

            animator.SetFloat("Horizontal", Mathf.Abs(movimientoHorizontal));
        }
        else
        {
            movimientoHorizontal = 0;
            rb2D.velocity = Vector2.zero;
            animator.SetFloat("Horizontal", 0);
        }
    }

    private void FixedUpdate()
    {
        if (!enDialogo)
            Mover(movimientoHorizontal * Time.fixedDeltaTime);
    }

    private void Mover(float mover)
    {
        Vector3 velocidadObjetivo = new Vector2(mover, rb2D.velocity.y);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, velocidadObjetivo, ref velocidad, suavizadoDeMovimiento);

        if (mover > 0 && !mirandoDerecha)
        {
            Girar();
        }
        else if (mover < 0 && mirandoDerecha)
        {
            Girar();
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("puertamuseo"))
        {
            // Detener el movimiento
            rb2D.velocity = Vector2.zero;
            this.enabled = false; // Desactivar el script de movimiento

            // Iniciar la transición de escena
            TransicionEscena3 transicion = collision.GetComponent<TransicionEscena3>();
            if (transicion != null)
            {
                transicion.IniciarTransicion();
            }
        }
        else if (collision.CompareTag("banca") && !enDialogo)
        {
            enDialogo = true;
            dialogoCoroutine = StartCoroutine(MostrarDialogos());
        }
    }

    private IEnumerator MostrarDialogos()
    {
        imagenDialogo.gameObject.SetActive(true);

        for (int i = 0; i < escenasDialogo.Count; i++)
        {
            EscenaDialogo escena = escenasDialogo[i];
            imagenDialogo.sprite = escena.imagen;
            textoDialogo.text = "";

            if (escena.sonido)
            {
                audioSource.clip = escena.sonido;
                audioSource.Play();
            }
            
            yield return StartCoroutine(MostrarTextoGradual(escena.dialogo, velocidadEscritura));
            audioSource.Stop();

            float tiempoEspera = Mathf.Max(escena.duracion - (escena.dialogo.Length * velocidadEscritura), 0) + escena.tiempoExtraLectura;
            yield return new WaitForSeconds(tiempoEspera);

            // Mostrar el botón "Saltar" después del primer diálogo
            if (i == 0 && botonSaltar != null)
            {
                botonSaltar.SetActive(true);
            }
        }

        TerminarDialogo();
    }

    private void TerminarDialogo()
    {
        imagenDialogo.gameObject.SetActive(false);
        textoDialogo.text = "";
        if (botonSaltar) botonSaltar.SetActive(false);
        enDialogo = false;
    }

    public void SaltarDialogo()
    {
        if (dialogoCoroutine != null)
        {
            StopCoroutine(dialogoCoroutine);
        }

        // Limpiamos la interfaz antes de mostrar el último diálogo
        imagenDialogo.gameObject.SetActive(true);
        textoDialogo.text = "";

        // Iniciar directamente el último diálogo
        dialogoCoroutine = StartCoroutine(MostrarUltimoDialogo());

        // Ocultar el botón Saltar
        if (botonSaltar != null)
        {
            botonSaltar.SetActive(false); // Aquí nos aseguramos de que el botón se oculte
            Debug.Log("Botón 'Saltar' ocultado.");
        }
        else
        {
            Debug.Log("El botón 'Saltar' no está vinculado correctamente.");
        }
    }

    public void SaltarAlUltimoDialogo()
    {
        if (botonSaltar) 
            botonSaltar.SetActive(false); // Ocultar el botón al saltar

        StopAllCoroutines(); // Detener todas las coroutines pendientes
        StartCoroutine(MostrarUltimoDialogo()); // Iniciar directamente el último diálogo
    }

    private IEnumerator MostrarUltimoDialogo()
    {
        EscenaDialogo ultimaEscena = escenasDialogo[escenasDialogo.Count - 1];
        
        imagenDialogo.sprite = ultimaEscena.imagen;
        textoDialogo.text = "";

        if (ultimaEscena.sonido)
        {
            audioSource.clip = ultimaEscena.sonido;
            audioSource.Play();
        }

        yield return StartCoroutine(MostrarTextoGradual(ultimaEscena.dialogo, velocidadEscritura));
        audioSource.Stop();

        yield return new WaitForSeconds(Mathf.Max(ultimaEscena.duracion - (ultimaEscena.dialogo.Length * velocidadEscritura), 0) + ultimaEscena.tiempoExtraLectura);
        
        TerminarDialogo();
    }

    private IEnumerator MostrarTextoGradual(string dialogo, float velocidad)
    {
        textoDialogo.text = "";
        foreach (char letra in dialogo)
        {
            textoDialogo.text += letra;
            yield return new WaitForSeconds(velocidad);
        }
    }
}
