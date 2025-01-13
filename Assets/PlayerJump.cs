using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 20f; // Jump force applied to the player
    private Rigidbody rb;
    private bool isGrounded;

    private void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing on " + gameObject.name);
        }
    }

    private void Update()
    {
        // Check for spacebar press and if the player is grounded
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key pressed.");
            
            if (isGrounded)
            {
                Jump();
            }
            else
            {
                Debug.Log("Player tried to jump but is not grounded.");
            }
        }
    }

    private void Jump()
    {
        // Apply upward force to the Rigidbody
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false; // Prevent jumping mid-air
        Debug.Log("Jump force applied: " + jumpForce);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player has landed on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Allow jumping again
            Debug.Log("Player is grounded.");
        }
    }
}