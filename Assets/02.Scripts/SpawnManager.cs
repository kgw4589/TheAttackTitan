using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager
{
    private GameObject[] _spawnPoints; 
    private List<GameObject> _scheduledMonster = new List<GameObject>();

    public void Init()
    {
        _spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }
    
    public void StartWave(WaveInfo waveInfo)
    {
        GameManager.Instance.LeftTitan += waveInfo.monsters.Count;
        
        for (int i = 0; i < waveInfo.monsters.Count; i++)
        {
            GameObject monster = waveInfo.monsters[i].monster;
            
            for (int j = 0; j < waveInfo.monsters[i].count; j++)
            {
                _scheduledMonster.Add(monster);
            }
        }

        GameManager.Instance.StartCoroutine(SpawnMonster(waveInfo.monsterCreateTimeRange));
    }
    
    private IEnumerator SpawnMonster(Vector2 createTimeRange)
    {
        for (int i = 0; i < _scheduledMonster.Count; i++)
        {
            int randomIndex = Random.Range(0, _spawnPoints.Length);

            GameObject monster = Object.Instantiate(_scheduledMonster[i]);
            monster.transform.position = _spawnPoints[randomIndex].transform.position;
            monster.SetActive(true);

            yield return new WaitForSeconds(GetCreateTime(createTimeRange));
        }
    }

    private float GetCreateTime(Vector2 range)
    {
        return Random.Range(range.x, range.y);
    }
}
