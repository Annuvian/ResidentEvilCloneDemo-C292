using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine_Edison : MonoBehaviour, IPickupable
{
    [SerializeField] private int ammoCount;
    [SerializeField] private int ammoReloadAmount;
    [SerializeField] private int ammoCapacity;
    [SerializeField] private string magType;

    public int AmmoCount { get => ammoCount; set => ammoCount = value; }
    public int AmmoCapacity { get => ammoCapacity; set => ammoCapacity = value; }
    public string MagType { get => magType; set => magType = value; }

    public void OnPickup(PlayerController_Edison player)
    {
        player.CurrentMag = this;
        gameObject.SetActive(false);
        gameObject.transform.SetParent(player.transform);

        //gameObject.transform.parent = player.transform;
    }

    public void OnDrop(Transform transform)
    {
        gameObject.SetActive(true);
        gameObject.transform.SetParent(null);

        //gameObject.transform.parent = null;
    }

    public void Reload()
    {
        ammoCapacity -= ammoReloadAmount;
        ammoCount = ammoReloadAmount;
    }

}
