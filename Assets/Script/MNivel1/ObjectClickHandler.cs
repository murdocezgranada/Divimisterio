using UnityEngine;

public class ObjectClickHandler : MonoBehaviour
{
    public ObjectFinderGame gameManager; // Referencia al script principal

    void OnMouseDown()
    {
        Debug.Log("Clic detectado en " + gameObject.name);
        if (gameManager != null)
        {
            gameManager.ObjectClicked(gameObject);
        }
    }
}
