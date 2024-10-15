using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// This class is used to define the events that will be used in the game
// Robbie methods of unity events
public static class MyEvents_Edison
{
    public static UnityEvent ZombieKilled = new UnityEvent();
    public static UnityEvent<int> AddPoints = new UnityEvent<int>();
	public static UnityEvent<string, int> CollectAmmo = new UnityEvent<string, int>();
}

