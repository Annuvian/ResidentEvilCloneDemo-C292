using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour, IPickupable
{
    [SerializeField] int maxCapacity;
    [SerializeField] int currentCount;
    [SerializeField] Enums.MagazineType magazineType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveRound()
    {
        if (currentCount > 0)
        {
            currentCount--;
        }
    }

    public Enums.MagazineType GetMagType()
    {
        return magazineType;
    }

    public void Pickup(PlayerController player)
    {
        gameObject.SetActive(false);
        Debug.Log("Magazine picked up!");
    }

    public int GetRounds()
    {
        return currentCount;
    }
}
