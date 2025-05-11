using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public WaveScriptable wave;
    
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

    public Text leftTitanText;
    public Text waveText;

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
                    GameOver();
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

    public void GameOver()
    {
        gameOverAction.Invoke();
    }
}
