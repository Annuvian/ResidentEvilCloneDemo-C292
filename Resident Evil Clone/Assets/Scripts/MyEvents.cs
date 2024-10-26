using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Notice our class is static. This means we cannot create any instances of it. It also means ANY class in our entire codebase has access to this class and can call methods from it.
public static class MyEvents
{
    // This is how we define a UnityEvent. Note it HAS to be static because the class itself is static.
    // Any single class in our codebase can now access and invoke and/or listen for the PickedUpItem event.
    // Notice that we're said the PickedUpItem event takes a string as an argument. This is important. Anytime we invoke this event we MUST pass in a string.
    // Likewise, for any class listening to this event, the method called in response to hearing the event being fired off MUST accept a string as an argument.
    // The string passed when the event was fired is the same one that will be received by the method(s) responding to hearing this event being triggered.
    public static UnityEvent<string> PickedUpItem = new UnityEvent<string>();

    // Another method that is accessible from anywhere. This one does NOT pass any data back and forth. It's simply something that happens with no data transmission associated with it.
    // Any class can listen for this event, and respond to it with any method that does NOT accept any arguments.
    public static UnityEvent ZombieKilled = new UnityEvent();
}