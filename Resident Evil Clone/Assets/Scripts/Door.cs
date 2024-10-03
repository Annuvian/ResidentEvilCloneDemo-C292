using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Reference for the Animator component.
    private Animator animator;
    // Reference to the AudioSource component.
    private AudioSource audioSource;
    // We'll use this field to store a reference to our lightbulb that will have color controlled by our door.
    [SerializeField] GameObject lightBulb;

    // Start is called before the first frame update
    void Start()
    {
        // Grab the reference to the Animator and store it.
        animator = gameObject.GetComponent<Animator>();
        // Grab the reference to the AudioSource and store it.
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This method will make the attached light green.
    // We'll make the light green when the door is closed.
    public void TurnLightGreen()
    {
        // Access the Light component on the lightBulb GameObject and set the color property to green.
        lightBulb.GetComponent<Light>().color = Color.green;
    }

    // This method will make the attached light red.
    // We'll make the light red when the door is anything other than closed.
    private void TurnLightRed()
    {
        // Access the Light component on the lightBulb and set the color field to green.
        lightBulb.GetComponent<Light>().color = Color.red;
    }

    // We'll use this method to trigger a sound effect for the door opening.
    // This method is going to be called inside the animation itself at a specific frame using Animation Events.
    private void DoorMoved()
    {
        // This is saying that we want to play a sound that emits from the audioSource.
        // It's also saying we want to just play the sound a single time, and the sound we want to play is the specific clip we pass in.
        audioSource.PlayOneShot(audioSource.clip);
        //audioSource.Play();
    }

    // We'll use this to force stop the audio that's playing.
    // We have this cause our sound effect we used sucks and has a bunch of background noise and stuff in it after the actual door sound.
    // So, after the door sound actually plays, we can just stop the rest of the playing.
    private void DoorStop()
    {
        // Stops all audio on the audioSource.
        audioSource.Stop();
    }

    // Event that is called when something enters the trigger zone of the door.
    void OnTriggerEnter(Collider col)
    {
        // Check to see if the thing that entered the zone is the player...
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("The player has entered the zone. Opening door...");
            // Tell the animator to fire off the ToggleDoor trigger action.
            // The animator will handle changing states by itself.
            animator.SetTrigger("ToggleDoor");
        }
    }

    // Event that is called when something exits the trigger zone of the door.
    void OnTriggerExit(Collider col)
    {
        // Check to see if the thing that entered the zone is the player...
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("The player has exited the zone. Closing door...");
            // Tell the animator to fire off the ToggleDoor trigger action.
            // The animator will handle changing states by itself.
            animator.SetTrigger("ToggleDoor");
        }
    }
}