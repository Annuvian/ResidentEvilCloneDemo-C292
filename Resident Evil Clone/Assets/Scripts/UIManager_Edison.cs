using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager_Edison : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private TextMeshProUGUI healthTxt;
    [SerializeField] private TextMeshProUGUI scoreTxt;

    private int globalScore = 0;
    public int GlobalScore { get { return globalScore; } }

    private void OnEnable()
    {
        MyEvents_Edison.AddPoints.AddListener(UpdateScore);
        PlayerController_Edison.OnPlayerDeath += GameOver;
        PlayerController_Edison.OnDamaged += UpdateHealth;
    }

    private void Start()
    {
        healthTxt = GameObject.Find("HealthTxt").GetComponent<TextMeshProUGUI>();
        scoreTxt = GameObject.Find("ScoreTxt").GetComponent<TextMeshProUGUI>();

        scoreTxt.text = "Score: " + globalScore;
        healthTxt.text = "Health: 0/100";
    }

    private void OnDisable()
    {
        PlayerController_Edison.OnPlayerDeath -= GameOver;
        PlayerController_Edison.OnDamaged -= UpdateHealth;
    }

    private void UpdateScore(int score)
    {
        globalScore += score;
        scoreTxt.text = "Score: " + globalScore;
    }

    private void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthTxt.text = "Health: " + currentHealth + "/" + maxHealth;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        Debug.Log("Game Over");
    }
}
