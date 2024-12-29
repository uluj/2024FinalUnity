using System.Collections;
using UnityEngine;

public class CollisionLogger : MonoBehaviour
{
    
    private PlayerHealth _playerHealth;
    private bool _isColliding = false;

    private void Start()
    {
        // Start the coroutine to log collisions every 2 seconds
        StartCoroutine(CollisionCheck());
        GetPlayerHealth();
    }

    void GetPlayerHealth()
    {
        _playerHealth = GameObject.Find("FirstPersonController").GetComponent<PlayerHealth>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Detect when the player collides with an object
        if (collision.gameObject.CompareTag("Player"))
        {
            _isColliding = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Detect when the collision ends
        if (collision.gameObject.CompareTag("Player"))
        {
            _isColliding = false;
        }
    }

    private IEnumerator CollisionCheck()
    {
        while (true)
        {
            if (_isColliding)
            {
                Debug.Log("Player is colliding!");
                _playerHealth.DecreaseHealth(50);
            }
            yield return new WaitForSeconds(2); // Wait for 2 seconds
        }
    }
}
