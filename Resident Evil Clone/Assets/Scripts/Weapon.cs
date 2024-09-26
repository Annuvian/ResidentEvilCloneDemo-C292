using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected int ammoCapacity;
    [SerializeField] protected int currentLoadedAmmo;
    [SerializeField] protected int currentSpareAmmo;
    [SerializeField] protected bool canFire;
    [Tooltip("The point in the barrel where the bullet spawns.")]
    [SerializeField] protected Transform firePoint;

    [SerializeField] protected Magazine magazine;

    [SerializeField] public Enums.MagazineType magazineType;

    private GameObject ammoText;
    // Start is called before the first frame update
    void Start()
    {
        ammoText = GameObject.FindWithTag("AmmoText");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Reload(Magazine newMag)
    {
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

    public virtual int CheckAmmo()
    {
        if (magazine != null)
        {
            return magazine.GetRounds();
        }
        else
        {
            return 0;
        }
    }

    public virtual void Fire()
    {
        if (magazine != null)
        {
            if (magazine.GetRounds() > 0)
            {
                magazine.RemoveRound();
                ammoText.GetComponent<TextMeshProUGUI>().text = "Ammo: " + CheckAmmo();
                RaycastHit hit;
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