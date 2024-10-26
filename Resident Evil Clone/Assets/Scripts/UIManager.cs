using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Tooltip("The text box to display how many zombies have been killed so far by the player.")]
    [SerializeField] TextMeshProUGUI zombieKillCountText;
    [Tooltip("The textbox to display how many points the player has accumulated so far.")]
    [SerializeField] TextMeshProUGUI scoreText;

    // Keeps track of how many zombies total have been killed by the player.
    private int zombiesKilled;

    // Start is called before the first frame update
    void Start()
    {
        // Let's call our loading method right here in Start. This is so that as soon as the game starts we can attempt to load any saved data that might exist.
        LoadBinaryData();

        // This is how we listen for events. Specifically, we are listening for the ZombieKilled event that is defined in the MyEvents class.
        // We're using the AddListener() method for the event which is what says, "This class is listening for this event to be fired.".
        // The argument for AddListener is the name of the method that will be called when this class "hears" the event being fired off.
        // So, whenever the ZombieKilled event is triggered, the UIManager will call its UpdateKillCount() method in response.
        MyEvents.ZombieKilled.AddListener(UpdateKillCount);

        // Force the zombie killed text to update to match the number of zombies killed (note this is 0 at the start of a new game, but maybe saved game data was loaded and it's different,
        // that is why we are updating the text here, just in case the number isn't 0 due to saved data being loaded).
        zombieKillCountText.text = "Zombies Killed: " + zombiesKilled;
    }

    // This method simply increases the count of killed zombies by one, updates the score, and the zombie kill count text.
    void UpdateKillCount()
    {
        // Increase the number of zombies killed by one.
        zombiesKilled++;
        // Same as zombiesKilled = zombiesKilled + 1;

        // Update the textbox that displays how many zombies have been killed.
        zombieKillCountText.text = "Zombies Killed: " + zombiesKilled;
        // Update the score text. NOTE our score doesn't really do anything, so we're just setting it to 100 now to show it works.
        scoreText.text = "Score: 100";

        // We are going to immediately save our data.
        SaveBinaryData();
    }

    // This method is used for saving data about our game progression as a binary file.
    // Remember, binary files are harder to read than simple JSON, text, or PlayerPrefs files.
    public void SaveBinaryData()
    {
        // Create a new instance of our GameData object we declared below.
        GameData data = new GameData();
        // Set the zombiesKilled field of the GameData object we just created to match how many zombies we've killed according to the UIManager.
        data.zombiesKilled = this.zombiesKilled;
        // Do the same thing but for the score.
        data.score = scoreText.text;

        // Create a new instance of the BinaryFormatter object. This is what we'll use to convert our save data to binary.
        BinaryFormatter formatter = new BinaryFormatter();
        // Let's generate and save the path we want to save to. We're using the same Application.persistentDataPath we used in the PlayerController.
        // For information about where this is actually located on the machine, refer to the PlayerController script.
        // NOTE: We can make the file type ANYTHING we want, even making custom file types. For this I've chosen to save our game data as a .potato file.
        // This file type doesn't actually exist, but we're making it anyway, so it exists now! Note this is commonly like saved as .dat or .data or something.
        // Who knows, maybe .potato files will catch on and be used by everybody one day?
        string path = Application.persistentDataPath + "/gameData.potato";
        // Let's create a new instance of a FileStream object. When we create it we need to tell it the location of the file it'll be interacting with, and how.
        // For saving, we're going to use FileMode.Create which will overwrite the file if it exists already, and create it if it doesn't exist.
        // This is where our gameData.potato file is actually being created.
        FileStream stream = new FileStream(path, FileMode.Create);

        // We're going to use our BinaryFormatter instance to serialize the data into a binary file.
        // All we do is call the Serialize method and give it a FileStream, and the data to binarize.
        formatter.Serialize(stream, data);
        // THIS IS VERY IMPORTANT!
        // We want to close the FileStream as soon as we're doing reading or writing the data. This helps prevent any errors, crashes, or data corruption.
        // As a rule of thumb, as soon as you're doing accessing data that's on disk, you should close the stream.
        stream.Close();
    }

    // This is what will load our data we have saved to disk.
    public void LoadBinaryData()
    {
        // Let's first create and save the path to our save data. Notice the file name and location are the same where we saved it, gameData.potato.
        string path = Application.persistentDataPath + "/gameData.potato";
        // Let's first check to see if this file actually exists at the path provided. (Our path and file name are in the same string).
        if (File.Exists(path))
        {
            // If it does exist, let's create a new instance of the BinaryFormatter again. Remember we did this when we saved the file to convert the save data to binary.
            // This time we're going to use it to conver the binary file back to something we can use.
            BinaryFormatter formatter = new BinaryFormatter();
            // Again, let's create a new FileStream that will access the file at the path provided by our path string, but this time, we're using FileMode.Open instead of .Create.
            // As you can probably guess, this is because we only want to open the file to do something with the data inside as opposed to creating or writing to a file.
            FileStream stream = new FileStream(path, FileMode.Open);

            // Create a new instance of GameData and populate it with the data from the stream we created in the last step.
            // Notice we are using the Deserialize() method of the BinaryFormatter to convert the data back into something readable and usable by our code.
            // We're also specifically telling it that the data is a GameData object. This is how it knows what data is in there, and what value is tied to what field.
            GameData data = formatter.Deserialize(stream) as GameData;
            // Again, VERY IMPORTANT, close the stream. If we don't do this, when we go to save data to this file later, it could corrupt it, not save, or cause all sorts of weird issues.
            // It might not do anything bad, but it's not a risk worth taking. Close your FileStreams!
            stream.Close();

            // Update the zombiesKilled field in the UIManager with the number that was stored for zombiesKilled in our save file.
            zombiesKilled = data.zombiesKilled;
            // Do the same with the score and update the text in one step.
            scoreText.text = data.score;
            // Update the textbox that displays how many zombies we've killed with our new zombie kill count obtained from the save file.
            zombieKillCountText.text = "Zombies Killed: " + zombiesKilled;
        }
    }
}

// A serializable (meaning it can be saved to disk) class.
[System.Serializable]
public class GameData
{
    // We only use this to store two things, an int and a string. Our zombies killed, and the score.
    // When we save our data we're actually saving the GameData object itself, and it is what has these fields in it.
    // We are not storing each thing individually, we're saving the GameData OBJECT which happens to have two fields which have values assigned to them.
    // When we load data, we're actually going to load the entire GameData object and it'll automatically find and assign the saved values to the fields. Pretty cool!
    public int zombiesKilled;
    public string score;
}