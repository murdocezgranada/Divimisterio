using UnityEngine;

public class ImagenClick : MonoBehaviour
{
    private void OnMouseDown()
    {
        FindObjectOfType<TransicionEscena1>().IniciarTransicion();
    }
}