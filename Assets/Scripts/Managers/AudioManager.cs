using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
   public static AudioManager Instance;
    
    [SerializeField] private AudioSource menuMusic;
    [SerializeField] private AudioSource gameMusic;
    [SerializeField] private AudioSource gameOverMusic;
    [SerializeField] private AudioSource sfxGem;
    
    [SerializeField] private GameObject mutedImage;
    
    private Scene _currentScene;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        _currentScene = SceneManager.GetActiveScene();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        mutedImage.SetActive(false);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene != _currentScene)
        {
            _currentScene = scene;
            
            switch (scene.name)
            {
                case "MainMenu":
                    PlayMenuMusic();
                    break;
                    
                case "Game":
                    PlayGameMusic();
                    break;
                    
                case "SelectCar":
                    PlayMenuMusic();
                    break;
                    
                default:
                    PlayMenuMusic();
                    break;
            }
        }
    }
    
    private void PlayMenuMusic()
    {
        StopAllMusic();
        menuMusic.Play();
    }
    
    private void PlayGameMusic()
    {
        StopAllMusic();
        gameMusic.Play();
    }
    
    private void StopAllMusic()
    {
        menuMusic.Stop();
        gameMusic.Stop();
        gameOverMusic.Stop();
    }
    
    public void ToggleSound()
    {
        bool shouldMute = !mutedImage.activeSelf;
        mutedImage.SetActive(shouldMute);
        MuteUnmuteAll(shouldMute);
    }
    
    private void MuteUnmuteAll(bool shouldMute)
    {
        menuMusic.mute = shouldMute;
        gameMusic.mute = shouldMute;
        gameOverMusic.mute = shouldMute;
        sfxGem.mute = shouldMute;
    }
    
    public void PlayGameOverMusic()
    {
        StartCoroutine(GameOverCoroutine());
    }
    
    private IEnumerator GameOverCoroutine()
    {
        StopAllMusic();
        yield return new WaitForSeconds(1f);
        gameOverMusic.Play();
    }
    
    public void PlayJemSound()
    {
        sfxGem.Play();
    }
    
}
