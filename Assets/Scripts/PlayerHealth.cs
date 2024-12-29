using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int _playerHealth=100;

    public void HealthCheck()
    {
        if (_playerHealth > 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public void DecreaseHealth(int damage)
    {
        _playerHealth -= damage;
        HealthCheck();
    }

}
