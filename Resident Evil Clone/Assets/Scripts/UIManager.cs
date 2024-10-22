using System.Collections;
using System.Collections.Generic;
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
        //PlayerPrefs.DeleteAll();

        if (PlayerPrefs.HasKey("Score"))
        {
            PlayerPrefs.DeleteKey("Score");
        }

        MyEvents.ZombieKilled.AddListener(UpdateKillCount);

        // zombiesKilled = PlayerPrefs.GetInt("ZombieKillCount", 0);

        if (PlayerPrefs.HasKey("ZombieKillCount"))
        {
            zombiesKilled = PlayerPrefs.GetInt("ZombieKillCount");
        }
        else
        {
            zombiesKilled = 0;
        }

        zombieKillCountText.text = "Zombies Killed: " + zombiesKilled;

        scoreText.text = PlayerPrefs.GetString("Score", "Score: 0");
    }

    void UpdateKillCount()
    {
        zombiesKilled++;
        // Same as zombiesKilled = zombiesKilled + 1;

        PlayerPrefs.SetInt("ZombieKillCount", zombiesKilled);

        zombieKillCountText.text = "Zombies Killed: " + zombiesKilled;
        scoreText.text = "Score: 100";
        PlayerPrefs.SetString("Score", scoreText.text);
    }
}