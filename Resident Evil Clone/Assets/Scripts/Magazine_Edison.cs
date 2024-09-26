using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine_Edison : MonoBehaviour, IPickupable
{
    [SerializeField] private GameObject magPrefab;
    [SerializeField] private int ammoCapacity;
    [SerializeField] private int currentAmmo;
    [SerializeField] private string magType;

    public int AmmoCapacity { get => ammoCapacity; set => ammoCapacity = value; }
    public int CurrentAmmo { get => currentAmmo; set =>  currentAmmo = value; }
    public string MagType { get => magType; set => magType = value; }

    public void OnPickup(PlayerController_Edison player)
    {
        player.CurrentMag = Instantiate(this);
        Destroy(gameObject);
    }

    public void OnDrop(Transform transform)
    {
        GameObject mag = Instantiate(magPrefab, transform.position, transform.rotation);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //UI
        }
    }
}
