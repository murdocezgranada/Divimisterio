using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonCambioEscena : MonoBehaviour
{
    void OnMouseDown()
    {
        SceneManager.LoadScene("Ciudad");
    }
}