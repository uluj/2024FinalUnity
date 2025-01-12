using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public int playerHealth=100;
    public Slider slider;
    public void HealthCheck()
    {
        if (playerHealth > 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public void DecreaseHealth(int damage)
    {
        playerHealth -= damage;
        slider.value = playerHealth; // Update the slider
        Invoke(nameof(HealthCheck), 0.5f);
    }

}
