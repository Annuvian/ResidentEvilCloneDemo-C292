using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save_Edison : MonoBehaviour
{
    //Get a reference to the player controller
    private PlayerController_Edison playerController;

    private void Awake()
    {
        //Find the player controller in the scene
        playerController = FindObjectOfType<PlayerController_Edison>();
    }

    private void Update()
    {
        //Handle saving and loading data
        if (Input.GetKeyDown(KeyCode.F5) && Input.GetKey(KeyCode.LeftShift))
        {
            SaveData();
            Debug.Log("Data Saved");
        }
        if (Input.GetKeyDown(KeyCode.F9) && Input.GetKey(KeyCode.LeftShift))
        {
            LoadData();
            Debug.Log("Data Loaded");
        }
    }
    /// <summary>
    /// Save the player's data
    /// </summary>
    public void SaveData()
    {
        //PlayerPrefs.SetInt("topScore", UIManager_Edison.intstance.GlobalScore);
        PlayerPrefs.SetInt("maxHealth", playerController.MaxHealth);
        PlayerPrefs.SetInt("currentHealth", playerController.CurrentHealth);
    }

    /// <summary>
    /// Load the player's data
    /// </summary>
    public void LoadData()
    {
        try
        {
            playerController.MaxHealth = PlayerPrefs.GetInt("maxHealth", 0);
            playerController.CurrentHealth = PlayerPrefs.GetInt("currentHealth", 0);
            UIManager_Edison.intstance.UpdateHealth(playerController.CurrentHealth,playerController.MaxHealth);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading data: " + e.Message);
        }
    }
}
