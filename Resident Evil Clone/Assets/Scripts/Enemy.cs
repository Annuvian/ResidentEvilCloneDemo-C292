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

    // How strong bullets knock the zombies back.
    [SerializeField] float knockBackForce;
    

    [Header("Object References")]
    [Tooltip("The target this enemy is trying to attack/reach.")]
    [SerializeField] Transform target;
    // Reference to the Rigidbody on the Enemy
    private Rigidbody rb;
    // Reference to a NavMeshAgent component.
    private NavMeshAgent agent;
    // Reference to the Animator component.
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // Set the current hp of the enemy to match the max hp.
        currentHealth = MaxHealth;
        // Initialize the agent field with the NavMeshAgent component on this enemy.
        agent = GetComponent<NavMeshAgent>();
        // Initialize the animator field with the Animator on the Enemy.
        animator = GetComponent<Animator>();

        // Find the player in the scene and set them as the target.
        target = GameObject.FindGameObjectWithTag("Player").transform;
        // Set the speed of the agent to match the moveSpeed field.
        // NOTE: When using a NavMeshAgent, they have their own speed field that you must set for any movement to be applied.
        agent.speed = moveSpeed;

        // Set the rb reference up.
        rb = GetComponent<Rigidbody>();
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

        // Check each frame to see if the zombie is in any taking damage animation.
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("takingDamage"))
        {
            // If they are, make them stop moving forward
            agent.speed = 0f;
        }
        else if (currentHealth > 0)
        {
            // Otherwise, move them normally.
            agent.speed = moveSpeed;
        }
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
        if (currentHealth == 0)
        {
            // Destroy the enemy.
            // This is how we trigger an event.
            // We're specifically triggering the ZombieKilled event from the MyEvents class by calling the Invoke() method on the event.
            // Any class that has been set up to listen to this event will "hear" it being fired off and respond according to what we have told it to do in response.
            MyEvents.ZombieKilled.Invoke();
            //Destroy(gameObject);
            // Randomly generate which death animation to play.
            int rand = Random.Range(0, 2);
            // Play the death animation.
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("crawling"))
            {
                animator.SetTrigger("Permadie");
            }
            else
            {
                if (rand == 0)
                {
                    animator.SetTrigger("Permadie");
                }
                else
                {
                    animator.SetTrigger("Backdie");
                }
            }

            // Stop the zombie from moving.
            agent.speed = 0;
        }
        else if (currentHealth > 1)
        {
            // If the zombie didn't die, play the animation for taking damage.
            animator.SetTrigger("Shot");
            // Push the zombie backward.
            rb.AddForce(transform.forward * -knockBackForce, ForceMode.Impulse);
        }
        // Will only get here if the current HP is 1.
        else
        {
            // Randomly generate either a 0 or 1.
            int rand = Random.Range(0, 2);
            // If the number was 0...
            if (rand == 0)
            {
                // Make the zombie fall down and start crawling.
                animator.SetTrigger("Fall Down");
            }
            // If the number was 1...
            else
            {
                // Trigger the hit animation.
                animator.SetTrigger("Shot");
            }
        }
    }
}