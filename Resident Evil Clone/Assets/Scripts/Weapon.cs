using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected int ammoCapacity;
    [SerializeField] protected int currentLoadedAmmo;
    [SerializeField] protected int currentSpareAmmo;
    [SerializeField] protected bool canFire;
    [Tooltip("The point in the barrel where the bullet spawns.")]
    [SerializeField] protected Transform firePoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void Reload()
    {
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
    }

    protected virtual void Fire()
    {
        // First, we'll create a RaycastHit variable, which is just a data container that holds lots data from a Raycast collision.
        RaycastHit hit;
        // This is combining the actual firing of the raycast with an if statement to see if it actually hit anything.
        // If the raycast doesn't hit anything, this will be false, and none of the logic inside the conditional statement will be called.
        // So we're calling the Raycast() method from the Physics class. There are several overloads for this method that take a different amount/type of arguments.
        // This particular one we're using takes 4 arguments.
        // (Vector3 location of where to start the raycast from, Vector3 direction of where to shoot the raycast to, Where to store the hit data, How far the raycast should travel).
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