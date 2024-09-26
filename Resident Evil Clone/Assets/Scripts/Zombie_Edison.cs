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
        Debug.Log("Zombie took damage");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
