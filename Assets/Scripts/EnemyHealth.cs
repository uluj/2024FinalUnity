using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;
    private AudioSource _audioSource;
    public void TakeDamage(float damageAmount)
    {
        Invoke("PlayAudio", 0.5f);
        maxHealth -= damageAmount;
        if(maxHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void PlayAudio()
    {
        _audioSource.Play();
        Debug.Log("Audio played after delay.");
    }
    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
