using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightSwitch : MonoBehaviour
{
    // This is how we make an event show up in the Inspector.
    // This is identical to how the OnPressed() events show up in the Inspector for Button objects that we can place in a Scene.
    // We are defining this method right here called PressedSwitch. All we have to do is simply drag any object into the list of listeners for PressedSwitch.
    // We do that right inside the Inspector, and that's also where we decide what action will be taken by that object when the event is triggered.
    [SerializeField] UnityEvent PressedSwitch;

    // This event will be fired whenever something enters the trigger area of this object (the collider).
    private void OnTriggerEnter(Collider other)
    {
        // First let's check to make sure the object that entered this collider is the Player.
        if (other.gameObject.tag == "Player")
        {
            // Invoke (trigger, fire, call, etc.) the PressedSwitch event. Anything that is listening for this event will hear it be fired off and respond according to what
            // behaviors were set in the Inspector.
            PressedSwitch.Invoke();
        }
    }
}