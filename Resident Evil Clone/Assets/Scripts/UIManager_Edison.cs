using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager_Edison : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private TextMeshProUGUI healthTxt;
    public static UIManager_Edison intstance;
    private int globalScore = 0;

    // public delegate void OnZombieDie(int score);
    // public static OnZombieDie zombieDeath;
    // public static event OnZombieDie zombieDeath2;

    void Start()
    {
        if(intstance == null){
            intstance = this;
        }else{
            Destroy(gameObject);
        }

        scoreTxt = GameObject.Find("ScoreTxt").GetComponent<TextMeshProUGUI>();
        healthTxt = GameObject.Find("HealthTxt").GetComponent<TextMeshProUGUI>();

        // zombieDeath += TestFunction;
        // zombieDeath += TestFunction1;
        // zombieDeath += TestFunction2;

        // zombieDeath += UpdateScore;
        

        // This is the same as zombieDeath?.Invoke(10);

        // if(zombieDeath != null){
        //     zombieDeath.Invoke(10);
        // }
    }
    // Update is called once per frame

    // public void TestFunction(){
    //     Debug.Log("Test");
    // }

    // public void TestFunction1(){
    //     Debug.Log("Test");
    // }

    // public void TestFunction2(){
    //     Debug.Log("Test");
    // }

    public void UpdateScore(int score){
        globalScore += score;
        scoreTxt.text = "Score: " + globalScore;
    }
}
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
