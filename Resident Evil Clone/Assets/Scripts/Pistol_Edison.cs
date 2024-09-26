using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol_Edison : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        canFire = true;
        ammoCapacity = 10;
        currentAmmo = ammoCapacity;
    }

    // Update is called once per frame

    protected override void Update()
    {
        base.Update();
    }

    protected override void Fire()
    {
        if (currentAmmo > 0 && canFire)
        {
            //Debug.Log("Pistol Fired");
            currentAmmo--;

            RaycastHit hit;
            if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, 100))
            {
                Debug.DrawRay(firePoint.position, firePoint.forward * hit.distance, Color.red, 2f);
                if (hit.transform.CompareTag("Zombie"))
                {
                    hit.transform.GetComponent<Zombie_Edison>().TakeDamage(damage);
                }
            }
        }
        
        if(currentAmmo <= 0)
        {
            Reload();
        }

        if (!canFire)
        {
            Debug.Log("Can't Fire");
        }

        if (currentAmmo <= 0)
        {
            Debug.Log("Out of Ammo");
        }
    }

    protected override void Reload()
    {
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        Debug.Log("Reloading...");
        canFire = false;

        yield return new WaitForSeconds(reloadTime);

        Debug.Log("Reloading Complete");
        canFire = true;
        currentAmmo = ammoCapacity;
    }
}
