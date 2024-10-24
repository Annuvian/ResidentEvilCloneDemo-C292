using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI zombieKillCountText;
    [SerializeField] TextMeshProUGUI scoreText;

    private int zombiesKilled;

    // Start is called before the first frame update
    void Start()
    {
        LoadBinaryData();

        MyEvents.ZombieKilled.AddListener(UpdateKillCount);

        zombieKillCountText.text = "Zombies Killed: " + zombiesKilled;
    }

    void UpdateKillCount()
    {
        zombiesKilled++;
        // Same as zombiesKilled = zombiesKilled + 1;

        zombieKillCountText.text = "Zombies Killed: " + zombiesKilled;
        scoreText.text = "Score: 100";

        SaveBinaryData();
    }

    public void SaveBinaryData()
    {
        GameData data = new GameData();
        data.zombiesKilled = this.zombiesKilled;
        data.score = scoreText.text;

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gameData.potato";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public void LoadBinaryData()
    {
        string path = Application.persistentDataPath + "/gameData.potato";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            zombiesKilled = data.zombiesKilled;
            scoreText.text = data.score;
            zombieKillCountText.text = "Zombies Killed: " + zombiesKilled;
        }
    }
}

[System.Serializable]
public class GameData
{
    public int zombiesKilled;
    public string score;
}