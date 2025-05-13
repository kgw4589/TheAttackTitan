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
                    StartCoroutine(StartWave());
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
        StartCoroutine(StartWave());
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(wave.waves[CurrentWave].waveStartDelay);

        for (int i = 0; i < wave.waves[CurrentWave].monsters.Count; i++)
        {
            LeftTitan += wave.waves[CurrentWave].monsters[i].count;
        }
        
        _spawnManager.StartWave(wave.waves[CurrentWave++]);
    }

    public void WinGame()
    {
        gameOverAction?.Invoke();

        resultText.text = "Victory";
        resultText.color = Color.green;
        
        resultText.gameObject.SetActive(true);
    }

    public void LoseGame()
    {
        gameOverAction?.Invoke();
        
        resultText.text = "Lose";
        resultText.color = Color.red;
        
        resultText.gameObject.SetActive(true);
    }
}
