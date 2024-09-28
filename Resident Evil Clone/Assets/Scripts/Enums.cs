using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums
{
    // Notice that this is a Static class. Let's recall what this means.
    // 1. We cannot instantiate instances of this class.
    // 2. We can access this class globally from anywhere and any class in our entire project.
    // 3. All methods and other data stored in a static class are also static.
    // We're using this specifically to store different enums for our game that might need to be
    // accessed from anywhere or by anything.

    // Here, we are creating a new enum called MagazineType.
    // We'll use this to store different types of magazines that exist in our game.
    public enum MagazineType
    {
        Rifle,
        Pistol,
        Shotgun
    }
}