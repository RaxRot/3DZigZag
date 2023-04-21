using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector] public bool CanPlay;

    [SerializeField] private GameObject tile;
    private Vector3 _currentPosition;
    [SerializeField] private float timeBetweenTileSpawn = 0.25f;
    [SerializeField] private int firstTilesSpawn = 10;
    private bool _isSpawnerIsActive;
    private bool _gameStarted;

    [SerializeField] private Material tileMat;
    [SerializeField] private Light dayLight;
    private Camera _camera;
    private bool _isCamColorLerp;
    private Color _cameraColor;
    private Color[] _tileColorDay;
    private Color _tileColorNight;
    private int _tileColorIndex;
    private Color _tileTrueColor;
    private float _timer;
    private float _timerInterval = 10f;
    private float _camLerpTimer;
    private float _camLerpInterval = 1f;
    private int direction=1;

    [Header("Car Settings")]
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject[] models;
    [SerializeField] private GameObject defaultModel;
    

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _currentPosition = new Vector3(2, 0, 3);

        for (int i = 0; i < firstTilesSpawn; i++)
        {
            CreateTiles();
        }
        
        ChosePlayer();
        
        _camera=Camera.main;

        _cameraColor = _camera.backgroundColor;
        _tileTrueColor = tileMat.color;
        _tileColorIndex = 0;
        _tileColorDay = new Color[3];
        _tileColorDay[0] = new Color(10 / 256f, 139/256, 203 / 256f);
        _tileColorDay[1] = new Color(10 / 256f, 200/256, 20 / 256f);
        _tileColorDay[2] = new Color(220 / 256f, 170/256, 45 / 256f);
        _tileColorNight = new Color(0, 8 / 256, 11 / 256);
        tileMat.color = _tileColorDay[0];

    }

    private void ChosePlayer()
    {
        for (int i = 0; i < models.Length; i++)
        {
            if (models[i].name==PlayerPrefs.GetString(TagManager.SELECTED_CAR_PREFS))
            {
                GameObject clone = Instantiate(models[i], player.modelHolder.position, player.modelHolder.rotation);
                clone.transform.parent = player.modelHolder;
                clone.transform.localScale = new Vector3(1f, 1f, 1f);
                defaultModel.SetActive(false);
            }
        }
    }
    

    private void OnDisable()
    {
        Instance = null;
        tileMat.color = _tileTrueColor;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) &&!_gameStarted)
        {
           StartGame();
        }
        
        CheckLerpTimer();
    }

    private void StartGame()
    {
        _gameStarted = true;
        CanPlay = true;
        UIManager.Instance.GameStarted();
    }

    public void ActivateTileSpawner()
    {
        if (!_isSpawnerIsActive)
        {
            _isSpawnerIsActive = true;
            StartCoroutine(_CreateTilesCo());
        }
    }

    private void CreateTiles()
    {
        Vector3 newPosition = _currentPosition;
        int rand = Random.Range(0, 100);
        if (rand<50)
        {
            newPosition.x += 1f;
        }
        else
        {
            newPosition.z += 1f;
        }

        _currentPosition = newPosition;
        Instantiate(tile, _currentPosition, Quaternion.identity);
    }

    private IEnumerator _CreateTilesCo()
    {
        yield return new WaitForSeconds(timeBetweenTileSpawn);
        
        CreateTiles();

        if (CanPlay)
        {
            StartCoroutine(_CreateTilesCo());
        }
    }

    public void PlayerDied()
    {
        CanPlay = false;
       
        _camera.backgroundColor = _cameraColor;
        tileMat.color = _tileTrueColor;
        UIManager.Instance.GameOver();
        AudioManager.Instance.PlayGameOverMusic();
    }

    private void CheckLerpTimer()
    {
        _timer += Time.deltaTime;
        if (_timer>_timerInterval)
        {
            _timer -= _timerInterval;
            _isCamColorLerp = true;
            _camLerpTimer = 0;
        }

        if (_isCamColorLerp)
        {
            _camLerpTimer += Time.deltaTime;
            float percent = _camLerpTimer / _camLerpInterval;
            if (direction==1)
            {
                _camera.backgroundColor = Color.Lerp(_cameraColor, Color.black, percent);
                tileMat.color = Color.Lerp(_tileColorDay[_tileColorIndex], _tileColorNight, percent);
                dayLight.intensity = 1f - percent;
            }
            else
            {
                _camera.backgroundColor = Color.Lerp(Color.black, _cameraColor, percent);
                tileMat.color = Color.Lerp(_tileColorNight, _tileColorDay[_tileColorIndex], percent);
                dayLight.intensity = percent;
            }

            if (percent>0.98f)
            {
                _camLerpTimer = 1f;
                direction *= -1;
                _isCamColorLerp = false;
                if (direction==-1)
                {
                    _tileColorIndex = Random.Range(0, _tileColorDay.Length);
                    
                }
            }
            
        }
    }
}
