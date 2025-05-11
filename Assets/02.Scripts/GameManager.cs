using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public WaveScriptable wave;
    public int currentWave = 0;

    public int leftTitan = 0;
    
    private SpawnManager _spawnManager = new SpawnManager();

    public Action gameOverAction;

    private void Awake()
    {
        _spawnManager.Init();
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
            
            _spawnManager.StartWave(wave.waves[currentWave++]);
        }
    }

    public void GameOver()
    {
        gameOverAction.Invoke();
    }
}
