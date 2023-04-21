using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject tapToStartText;
    [SerializeField] private TMP_Text coinsText;
    private int _currentCoins;

    [SerializeField] private GameObject deadScreen;
    [SerializeField] private TMP_Text coinsDeadText;

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(TagManager.COINS_PREFS))
        {
            _currentCoins = PlayerPrefs.GetInt(TagManager.COINS_PREFS);
        }
        else
        {
            _currentCoins = 0;
        }

        deadScreen.SetActive(false);
        ShowCoins(); 
    }

    public void GameStarted()
    {
        tapToStartText.SetActive(false);
    }

    public void AddCoin()
    {
        _currentCoins++;
       ShowCoins();
    }

    public void GameOver()
    {
        deadScreen.SetActive(true);
        PlayerPrefs.SetInt(TagManager.COINS_PREFS,_currentCoins);
        coinsDeadText.text = "Coins: " + PlayerPrefs.GetInt(TagManager.COINS_PREFS);
    }

    private void ShowCoins()
    {
        coinsText.text = "Coins: " + _currentCoins;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
