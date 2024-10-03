using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    [Tooltip("The maximum amount of rounds this weapon can hold.")]
    [SerializeField] protected int ammoCapacity;
    [Tooltip("The number of rounds currently in the weapon.")]
    [SerializeField] protected int currentLoadedAmmo;
    [Tooltip("The current number of additional rounds available that aren't currently loaded.")]
    [SerializeField] protected int currentSpareAmmo;
    [Tooltip("Determines if we can or cannot fire the weapon.")]
    [SerializeField] protected bool canFire;
    [Tooltip("The type of magazines this weapon can accept.")]
    [SerializeField] public Enums.MagazineType magazineType;

    [Header("Object References")]
    [Tooltip("The point where the bullet spawns at.")]
    [SerializeField] protected Transform firePoint;
    [Tooltip("The magazine currently loaded in this weapon.")]
    [SerializeField] protected Magazine magazine;

    // We'll use this to store a reference to the UI object that displays ammo remaining.
    [SerializeField] TextMeshProUGUI ammoText;

    // Start is called before the first frame update
    void Start()
    {
        // When this weapon spawns into the world it will search the entire scene for any
        // object with the tag "AmmoText" and store a reference to it in the ammoText field.
        //ammoText = GameObject.FindGameObjectWithTag("AmmoText");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This is where our reloading logic will be handled.
    // Notice that when we reload, a magazine must be passed as an argument
    // so that our weapon knows which magazine is in it and can reference it.
    public virtual void Reload(Magazine newMag)
    {
        // Update the value of the magazine field with the passed Magazine.
        magazine = newMag;
        /*
        if (currentLoadedAmmo < ammoCapacity)
        {
            if (currentSpareAmmo > 0)
            {
                int bulletsToLoad = ammoCapacity - currentLoadedAmmo;
                if (currentSpareAmmo >= bulletsToLoad)
                {
                    currentLoadedAmmo = ammoCapacity;
                    currentSpareAmmo -= bulletsToLoad;
                }
                else
                {
                    currentLoadedAmmo = currentLoadedAmmo + currentSpareAmmo;
                }
            }
        }
        */
    }

    // This will simply return how many current rounds are loaded into the magazine that in the gun currently.
    public virtual int CheckAmmo()
    {
        // First make sure the weapon actually has a magazine inserted...
        if (magazine != null)
        {
            // Call GetRounds() which returns an int.
            return magazine.GetRounds();
        }
        else
        {
            // In all other situations, return 0.
            return 0;
        }
    }

    // Here is where we handle actually firing a round from the Weapon.
    public virtual void Fire()
    {
        // First, we need to make sure a magazine is actually in the weapon.
        if (magazine != null)
        {
            // Then we need to make sure the magazine actually has rounds in it.
            if (magazine.GetRounds() > 0)
            {
                // We will remove a round from the loaded magazine by calling the RemoveRound() method.
                // This removed round is essentially the round that will be fired.
                magazine.RemoveRound();
                // Update the current ammo in the weapon.
                ammoText.text = "Ammo: " + CheckAmmo();
                // Container to store raycast hit data.
                RaycastHit hit;
                // Fire a raycast out from the firePoint in the forward direction. We then store the data. It has a range of 500m.
                if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, 500))
                {
                    // So, if our raycast hit anything, let's first draw a line that can be seen in the Scene (but not Game) view.
                    // This is optional but it allows us to actually see where the laser is.
                    // DrawRay() is a method in the Debug class. It takes 4 arguments.
                    // (Where to start the ray, the direction to fire it in (notice we multiply it by a distance so it's limited to a certain length), the color, the duration in seconds it will be displayed.
                    Debug.DrawRay(firePoint.position, firePoint.forward * hit.distance, Color.red, 2f);
                    // Check to see if the object the ray hit is a Zombie.
                    // NOTE: CompareTag("Zombie") == tag == "Zombie"
                    // Remember, the hit variable is storing the thing we hit. So we are accessing the transform of what was hit, and checking the tag.
                    if (hit.transform.CompareTag("Zombie"))
                    {
                        // Grab the Enemy script on the Enemy we hit, and call its TakeDamage() method, passing in the damage to deal (1 in this case).
                        hit.transform.GetComponent<Enemy>().TakeDamage(1);
                    }
                }
            }
        }
    }
}