using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class MyEvents
{
    public static UnityEvent<string> PickedUpItem = new UnityEvent<string>();

    public static UnityEvent ZombieKilled = new UnityEvent();
}