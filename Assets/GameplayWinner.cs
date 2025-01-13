using UnityEngine;

public class GameplayWinner : MonoBehaviour
{
    [SerializeField] private GameObject canvasToActivate;
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the Player tag
        if (other.CompareTag("Player"))
        {
            if (canvasToActivate != null)
            {
                canvasToActivate.SetActive(true); // Activate the canvas
                Debug.Log("Canvas activated!");
            }
            else
            {
                Debug.LogError("No Canvas assigned to TriggerCanvasActivator.");
            }
        }
    }
}
