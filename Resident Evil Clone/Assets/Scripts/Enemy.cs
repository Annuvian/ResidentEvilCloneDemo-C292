using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // For a description of the Header attribute, see the PlayerController class.
    [Header("Enemy Stats")]
    // For a description of the Tooltip attribute, see the PlayerController class.
    [Tooltip("The move speed of the enemy in meters per second.")]
    [SerializeField] float moveSpeed = 5f;
    [Tooltip("The maximum health this enemy can have.")]
    [SerializeField] float MaxHealth = 5f;
    // The current health of the enemy.
    private float currentHealth;

    [Header("Object References")]
    [Tooltip("The target this enemy is trying to attack/reach.")]
    [SerializeField] Transform target;
    // Reference to a NavMeshAgent component.
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the agent field with the NavMeshAgent component on this enemy.
        agent = GetComponent<NavMeshAgent>();
        // Set the speed of the agent to match the moveSpeed field.
        // NOTE: When using a NavMeshAgent, they have their own speed field that you must set for any movement to be applied.
        agent.speed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Every frame we are updating the point the enemy is trying to reach with the player's current position.
        // If we didn't do this in Update() the point the enemy is trying to reach wouldn't be updating with the player as the player moves around.
        // For example if you called SetDestination() in Start(), the enemy would spawn and go to the location the player WAS at when the enemy spawned
        // even if the player has since moved away from that spot.
        // So this makes the enemy continually track the player as they run around.
        agent.SetDestination(target.position);
    }

    // Handles dealing damage to the enemy.
    public void TakeDamage(float damage)
    {
        // Use a lot of Debug.Log statements! They help check for any logic errors, make sure your code is doing what you expect it to do,
        // as well as help you find exactly what part is causing issues.
        // This simply prints a string to the console window displaying how much damage the enemy took.
        Debug.Log("Zombie took Damage: " +  damage);
        // Subtract the damage dealt from the enemy health.
        currentHealth -= damage;
        // Check to see if the health of the enemy is less than or equal to 0.
        if (currentHealth <= 0)
        {
            // Destroy the enemy.
            MyEvents.ZombieKilled.Invoke();
            Destroy(gameObject);
        }
    }
}