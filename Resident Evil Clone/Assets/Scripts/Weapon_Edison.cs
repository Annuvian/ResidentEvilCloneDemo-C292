using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon_Edison : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] protected Transform fpsCamera;

    [SerializeField] protected Magazine_Edison magazine;
    [SerializeField] protected float fireRate;
    [SerializeField] protected bool canFire;

    [SerializeField] protected float damage;
    [SerializeField] protected float reloadTime;
    public Magazine_Edison Magazine { get => magazine; set => magazine = value; }

    protected virtual void Start()
    {
        canFire = true;
    }

    protected virtual void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    protected virtual void Fire() { }
    protected virtual void Reload() { }

}
