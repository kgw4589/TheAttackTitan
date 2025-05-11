using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public WaveScriptable wave;
    
    public Text leftTitanText;
    public Text waveText;
    public Text resultText;
    
    private int _currentWave = 0;

    public int CurrentWave
    {
        get
        {
            return _currentWave;
        }
        set
        {
            _currentWave = value;

            waveText.text = $"웨이브 : {_currentWave} / {wave.waves.Count}";
        }
    }

    private int _leftTitan = 0;

    public int LeftTitan
    {
        get
        {
            return _leftTitan;
        }
        set
        {
            _leftTitan = value;
            
            leftTitanText.text = $"남은 거인 : {_leftTitan}";

            if (_leftTitan <= 0)
            {
                if (_currentWave < wave.waves.Count)
                {
                    StartCoroutine(StartGame());
                }
                else
                {
                    WinGame();
                }
            }
        }
    }
    
    private SpawnManager _spawnManager = new SpawnManager();

    public Action gameOverAction;

    private void Awake()
    {
        _spawnManager.Init();

        CurrentWave = 0;
    }
    
    private void Start()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        for (int i = 0; i < wave.waves.Count; i++)
        {
            yield return new WaitForSeconds(wave.waves[i].waveStartDelay);
            
            _spawnManager.StartWave(wave.waves[CurrentWave++]);
        }
    }

    public void WinGame()
    {
        gameOverAction?.Invoke();

        resultText.text = "Victory";
        resultText.gameObject.SetActive(true);
    }

    public void LoseGame()
    {
        gameOverAction?.Invoke();
        
        resultText.text = "Lose";
        resultText.gameObject.SetActive(true);
    }
}
