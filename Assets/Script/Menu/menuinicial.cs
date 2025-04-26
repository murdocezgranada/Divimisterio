using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuinicial : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
        Application.Quit();
        System.Diagnostics.Process.GetCurrentProcess().Kill(); // Forzar cierre en Windows
        #elif UNITY_ANDROID
        Application.Quit();
        Debug.Log("El juego debería cerrarse en Android, pero el SO puede mantenerlo en segundo plano.");
        #endif
    }
}
