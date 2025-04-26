using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 originalScale;
    public float hoverScale = 1.2f;  // Efecto de agrandado en PC
    public float clickScale = 0.9f;  // Efecto al hacer clic/tocar
    public float animSpeed = 0.15f;  // Velocidad de la animación

    private Image buttonImage;
    public Color hoverColor = Color.yellow;
    private Color originalColor;

    public bool isCorrectButton = false; //  Marcar el botón correcto en el Inspector
    public float hintDelay = 30f; //  Tiempo antes de la pista

    void Start()
    {
        originalScale = transform.localScale;
        buttonImage = GetComponent<Image>();
        originalColor = buttonImage.color;

        if (isCorrectButton)
        {
            StartCoroutine(HintEffect()); //  Inicia la espera para la pista
        }
    }

    // 🖱️ Para PC - Efecto de hover
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsTouchDevice()) 
            LeanTween.scale(gameObject, originalScale * hoverScale, animSpeed).setEaseOutQuad();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsTouchDevice()) 
            LeanTween.scale(gameObject, originalScale, animSpeed).setEaseOutQuad();
    }

    // 📱 Para móviles - Efecto al tocar
    public void OnPointerDown(PointerEventData eventData)
    {
        LeanTween.scale(gameObject, originalScale * clickScale, animSpeed / 2).setEaseOutQuad();
        buttonImage.color = hoverColor;  // Cambio de color al tocar
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        LeanTween.scale(gameObject, originalScale, animSpeed).setEaseOutBounce();
        buttonImage.color = originalColor;  // Vuelve al color original
    }

    //  Pista visual tras 30s: el botón empieza a parpadear
    IEnumerator HintEffect()
    {
        yield return new WaitForSeconds(hintDelay); //  Espera 30s

        while (true)
        {
            LeanTween.color(buttonImage.rectTransform, Color.green, 0.5f).setLoopPingPong(3);
            yield return new WaitForSeconds(3f); //  Cada 3s repite la animación
        }
    }

    // 🔍 Detecta si es un dispositivo táctil
    private bool IsTouchDevice()
    {
        return Input.touchSupported;
    }
}