using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLight : MonoBehaviour
{
    [Tooltip("The lights associated with this light bulb.")]
    [SerializeField] GameObject[] lightSources;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This method will be used to toggle the lights on and off.
    public void ToggleLights()
    {
        // Go through each lightSource for this light.
        foreach (GameObject lightSource in lightSources)
        {
            // Set the object to active or inactive. Whatever the opposite of it currently is.
            // isActiveAndEnabled returns true if it's on, and false if it's off.
            // So by using !isActiveAndEnabled, if the light is on, this will return false, turning it off.
            // And if it's off, it'll return true and turn it back on.
            lightSource.SetActive(!isActiveAndEnabled);
        }
    }

    // This method will just change the color of the light to match the passed color.
    public void SetColor(Color color)
    {
        // Use a foreach loop to go through each item in the lightSources array.
        foreach (GameObject light in lightSources)
        {
            // Make sure there is a Light component on the object.
            if (light.GetComponent<Light>() != null)
            {
                // Set the color of the Light to match the color passed in for the color parameter.
                light.GetComponent<Light>().color = color;
            }
        }
    }
}