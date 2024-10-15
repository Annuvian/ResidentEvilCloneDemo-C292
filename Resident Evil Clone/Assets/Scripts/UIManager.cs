using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI zombieKillCountText;

    private int zombiesKilled = 0;

    // Start is called before the first frame update
    void Start()
    {
        MyEvents.ZombieKilled.AddListener(UpdateKillCount);
    }

    void UpdateKillCount()
    {
        zombiesKilled++;
        // Same as zombiesKilled = zombiesKilled + 1;

        zombieKillCountText.text = "Zombies Killed: " + zombiesKilled;
    }
}