using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour, IPickupable
{
    // Notice that our Magazine class inherits from MonoBehaviour AND implements IPickupable.
    // In C# you can only have a class inherit from one other class, but it can implement as many interfaces as you'd like.
    [Header("Magazine Specs")]
    [Tooltip("The maximum number of rounds this magazine can hold.")]
    [SerializeField] int maxCapacity;
    [Tooltip("The current number of rounds loaded into this magazine.")]
    [SerializeField] int currentCount;
    [Tooltip("The type of weapon this magazine is for.")]
    [SerializeField] Enums.MagazineType magazineType;

    // Used to get a reference to the context text.
    private ContextualText contextText;

    // Start is called before the first frame update
    void Start()
    {
        // Search for and find the contextText object.
        contextText = GameObject.FindGameObjectWithTag("ContextText").GetComponent<ContextualText>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Simply removes a round from the magazine (such as when firing it).
    public void RemoveRound()
    {
        // Make sure it has at least 1 bullet in it.
        if (currentCount > 0)
        {
            // Subtract 1 from the currentCount and update the value.
            // For those who haven't used this before:
            // currentCount-- == currentCount = currentCount - 1;
            currentCount--;
        }
    }

    // We'll use this to directly return a MagazineType.
    public Enums.MagazineType GetMagType()
    {
        // Simply returns the type of magazine this is.
        // We'll use this to make sure this magazine can be inserted into a particular weapon during an attempted reload.
        return magazineType;
    }

    // This is the Pickup method that we MUST declare because this class implements IPickupable.
    public void Pickup(PlayerController player)
    {
        // Whenever this object is picked up, we will set it to false.
        // This doesn't destroy the object, but makes it invisible and will stop calling any code in Update().
        // We don't destroy the object because we still want to be able to reference it in the player's inventory so we can use it.
        gameObject.SetActive(false);
        Debug.Log("Magazine picked up!");
        contextText.UpdateText("Magazine picked up!");
    }

    // Returns the number of currently loaded rounds.
    public int GetRounds()
    {
        return currentCount;
    }
}