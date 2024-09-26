using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupable
{
    // Properties
    // Everything is default to public
    int AmmoCapacity { get; set; }
    int CurrentAmmo { get; set; }
    string MagType { get; set; }
    void OnPickup(PlayerController_Edison player);
    void OnDrop(Transform transform);
}
