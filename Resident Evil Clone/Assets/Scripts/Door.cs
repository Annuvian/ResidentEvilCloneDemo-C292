using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;
    [SerializeField] GameObject lightBulb;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnLightGreen()
    {
        lightBulb.GetComponent<Light>().color = Color.green;
    }

    private void TurnLightRed()
    {
        lightBulb.GetComponent<Light>().color = Color.red;
    }

    private void DoorMoved()
    {
        audioSource.PlayOneShot(audioSource.clip);
        //audioSource.Play();
    }

    private void DoorStop()
    {
        audioSource.Stop();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            animator.SetTrigger("ToggleDoor");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            animator.SetTrigger("ToggleDoor");
        }
    }
}