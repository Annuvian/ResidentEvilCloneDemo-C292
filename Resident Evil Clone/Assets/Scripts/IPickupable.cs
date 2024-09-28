using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupable
{
    // We've looked at abstract classes, static classes, and obviously normal classes so far.
    // Notice that this isn't a class at all. It's an interface. What's an interface?
    // 1. You cannot create instances of it directly.
    // 2. Other classes can "implement" this interface.
    // 3. Anything that implements this interface MUST define all methods that are declared here in the interface.

    // For example, we have this Pickup method. Well this is IPickupable, so anything that implements the interface
    // should be something that can be picked up. So we'll declare a Pickup method that takes in a PlayerController as an argument.
    // Notice there's no logic for the method here. This is simply saying:
    // "If you implement from this interface you MUST define your own Pickup method and it MUST take a PlayerController as an argument.
    public void Pickup(PlayerController player);
}