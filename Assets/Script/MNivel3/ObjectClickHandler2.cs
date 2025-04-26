using UnityEngine;

public class ObjectClickHandler2 : MonoBehaviour
{
    public ObjectFinderGame2 gameManager; // Referencia al script principal

    void OnMouseDown()
    {
        Debug.Log("Clic detectado en " + gameObject.name);
        if (gameManager != null)
        {
            gameManager.ObjectClicked(gameObject);
        }
    }
}