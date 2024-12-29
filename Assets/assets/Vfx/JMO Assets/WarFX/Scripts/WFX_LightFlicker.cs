using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class WFX_LightFlicker : MonoBehaviour
{
    public float time = 0.05f;

    private float timer;
    private Coroutine flickerCoroutine;
    private bool isFlickering = false; // Control flag to stop the coroutine

    void Start()
    {
        timer = time;
    }

    void Update()
    {
        // Check if the Fire1 button is pressed
        if (Input.GetButtonDown("Fire1"))
        {
            if (!isFlickering)
            {
                // Start flickering when the button is pressed
                flickerCoroutine = StartCoroutine(Flicker());
                isFlickering = true;
            }
            else
            {
                // Stop flickering if it's already running
                StopFlickering();
            }
        }
    }

    void StopFlickering()
    {
        isFlickering = false; // Set flag to false
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine); // Stop the coroutine
            flickerCoroutine = null;
        }
        GetComponent<Light>().enabled = false; // Ensure the light turns off
    }

    IEnumerator Flicker()
    {
        while (isFlickering) // Check the control flag
        {
            GetComponent<Light>().enabled = !GetComponent<Light>().enabled;

            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null; // Wait until the next frame
            }

            timer = time; // Reset the timer
        }
    }
}
