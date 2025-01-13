using System.Collections;
using UnityEngine;

public class CollisionLogger : MonoBehaviour
{
    public Animator animator;
    private PlayerHealth _playerHealth;
    public int attackDamage = 50;     // Damage per attack
    public float attackCooldown = 3.0f; // Time in seconds before the enemy can attack again

    private bool canEnemyAttack = true; // Tracks whether the enemy can attack

    private void Start()
    {
        // Find and set the player's health component
        GetPlayerHealth();
    }

    void GetPlayerHealth()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger collider
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered attack range!");

            // Attempt an attack if the enemy can attack
            if (canEnemyAttack)
            {
                Attack();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Continuously attack when the player stays in the trigger and cooldown allows
        if (other.CompareTag("Player") && canEnemyAttack)
        {
            Attack();
        }
    }

    private void Attack()
    {
        animator?.SetBool("ZombieBite", true);
        Debug.Log("Player is being attacked!");
        _playerHealth.DecreaseHealth(attackDamage);
        canEnemyAttack = false; // Disable attack
        StartCoroutine(ResetAttackCooldown()); // Start cooldown timer
    }

    private IEnumerator ResetAttackCooldown()
    {
        Debug.Log("Attack cooldown started...");
        yield return new WaitForSeconds(attackCooldown);
        canEnemyAttack = true; // Re-enable attack
        Debug.Log("Enemy can attack again.");
    }

    private void OnTriggerExit(Collider other)
    {
        animator?.SetBool("ZombieBite", false);
        // Log when the player exits the trigger collider
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left attack range.");
        }
    }
}
