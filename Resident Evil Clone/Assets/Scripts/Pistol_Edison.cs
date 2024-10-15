using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol_Edison : Weapon_Edison
{
    protected override void Start()
    {
        canFire = true;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Fire()
    {
        if(currentMag == null){
            Debug.Log("No mag");
            return;
        }

        if (currentMag.AmmoCount > 0 && canFire)
        {
            //Debug.Log("Pistol Fired");
            AudioManager_Edison.instance.PlayOneShot(AudioManager_Edison.instance.rayGun);
            currentMag.AmmoCount--;

            RaycastHit hit;
            Debug.DrawRay(fpsCamera.position, fpsCamera.forward * 100, Color.red, 2f);
            if (Physics.Raycast(fpsCamera.position, fpsCamera.forward, out hit, 100))
            {
                if (hit.transform.CompareTag("Zombie"))
                {
                    hit.transform.GetComponent<Zombie_Edison>().TakeDamage(damage);
                }
            }
        }
        
        if(currentMag.AmmoCount <= 0)
        {
            Reload();
        }

        if (!canFire)
        {
            //Debug.Log("Can't Fire");
        }

        if (currentMag.AmmoCount <= 0)
        {
            //Debug.Log("Out of Ammo");
        }
    }

    protected override void Reload()
    {
        if(magazine != null)
        {
            StartCoroutine(ReloadCoroutine());
        }
        else
        {
            Debug.Log("No Magazine");
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        Debug.Log("Reloading...");
        canFire = false;

        yield return new WaitForSeconds(reloadTime);

        Debug.Log("Reloading Complete");
        canFire = true;
        //currentAmmo = ammoCapacity;
        currentMag.Reload();
    }
}
