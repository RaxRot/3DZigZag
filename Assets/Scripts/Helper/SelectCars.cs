using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCars : MonoBehaviour
{
    [SerializeField] private GameObject[] cars;
    private int _carIndex;

    [SerializeField] private TMP_Text coins;
    private int _currentCoins;

    [SerializeField] private GameObject notEnoughMoney;
    [SerializeField] private float timeToShowWarning = 2f;

    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject unlockButton;
    
    private void Start()
    {
        //1 true 0 false
        PlayerPrefs.SetInt(cars[0].name, 1);
        notEnoughMoney.SetActive(false);
        UpdateCharacters();
        ShowCoins();
    }

    private void ShowCoins()
    {
        if (PlayerPrefs.HasKey(TagManager.COINS_PREFS))
        {
            _currentCoins = PlayerPrefs.GetInt(TagManager.COINS_PREFS);
            _currentCoins = 237;
            coins.text = "Coins: " +_currentCoins;
        }
        else
        {
            coins.text = "Coins:" + 0;
        }
    }

    private void UpdateCharacters()
    {
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].SetActive(false);
        }
        cars[_carIndex].SetActive(true);
        
        ShowCharInfo();
    }

    private void ShowCharInfo()
    {
        if (PlayerPrefs.GetInt(cars[_carIndex].name)==1)
        {
            playButton.SetActive(true);
            unlockButton.SetActive(false);
        }
        else
        {
            playButton.SetActive(false);
            unlockButton.SetActive(true);
            unlockButton.GetComponentInChildren<TextMeshProUGUI>().text =
                "Price:\n" + cars[_carIndex].GetComponent<CarPrice>().GetPrice() + "\nCoins";
        }
    }

    public void UnlockPlayer()
    {
        if (_currentCoins>=cars[_carIndex].GetComponent<CarPrice>().GetPrice())
        {
            _currentCoins -= cars[_carIndex].GetComponent<CarPrice>().GetPrice();
            coins.text = "Coins: " +_currentCoins;
            PlayerPrefs.SetInt(TagManager.COINS_PREFS,_currentCoins);
            PlayerPrefs.SetInt(cars[_carIndex].name,1);
            
            UpdateCharacters();
        }
        else
        {
            ShowWarning();
        }
    }

    public void Play()
    {
        PlayerPrefs.SetString(TagManager.SELECTED_CAR_PREFS,cars[_carIndex].name);
        SceneManager.LoadScene("Game");
    }

    public void MoveLeft()
    {
        _carIndex--;
        if (_carIndex<=0)
        {
            _carIndex = 0;
        }
        
        UpdateCharacters();
    }

    public void MoveRight()
    {
        _carIndex++;
        if (_carIndex>=cars.Length)
        {
            _carIndex = cars.Length - 1;
        }
        
        UpdateCharacters();
    }

    private void ShowWarning()
    {
        StartCoroutine(_ShowWarningCo());
    }

    private IEnumerator _ShowWarningCo()
    {
        notEnoughMoney.SetActive(true);
        yield return new WaitForSeconds(timeToShowWarning);
        notEnoughMoney.SetActive(false);
    }
}
