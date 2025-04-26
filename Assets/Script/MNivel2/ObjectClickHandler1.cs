using UnityEngine;

public class ObjectClickHandler1 : MonoBehaviour
{
    public ObjectFinderGame1 gameManager; // Referencia al script principal

    void OnMouseDown()
    {
        Debug.Log("Clic detectado en " + gameObject.name);
        if (gameManager != null)
        {
            gameManager.ObjectClicked(gameObject);
        }
    }
}