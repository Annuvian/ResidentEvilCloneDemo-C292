using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie_Edison : MonoBehaviour
{
    [SerializeField] private Transform target; //Player
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float maxHealth = 5;
    [SerializeField] private int scoreValue = 10;

    private float currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        target = GameObject.Find("Player").transform;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);
    }

    public void TakeDamage(float damage)
    {
        //Debug.Log("Zombie took damage");
        currentHealth -= damage;
        AudioManager_Edison.instance.PlayRandomOneShot(AudioManager_Edison.instance.zombieDamage);
        if (currentHealth <= 0)
        {
            //Invoke takes in the parameter of the unity event
            MyEvents_Edison.AddPoints.Invoke(scoreValue);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;

            Vector3 direction = transform.position - player.transform.position;

            player.GetComponent<PlayerController_Edison>().Damage(1, direction.normalized, 500);

            //PlayerController_Edison.OnPlayerDeath2();
        }
    }
}
