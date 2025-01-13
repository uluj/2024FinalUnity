using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public GameObject loseCanvas;
    private AudioSource _audioSource;
    public int playerHealth=100;
    public Slider slider;
    public void HealthCheck()
    {
        if (playerHealth <= 0)
        {
            // Activate the loseCanvas
            loseCanvas.SetActive(true);

            // Reload the scene after 2 seconds
            Invoke(nameof(ReloadScene), 5f);
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void DecreaseHealth(int damage)
    {
        _audioSource.Play();
        playerHealth -= damage;
        slider.value = playerHealth; // Update the slider
        Invoke(nameof(HealthCheck), 0.5f);
    }

    private void Start()
    {
        _audioSource = GetComponentInChildren<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("No AudioSource found in children of " + gameObject.name);
        }
        else
        {
            Debug.Log("AudioSource found in children of " + gameObject.name);
        }
    }
}
