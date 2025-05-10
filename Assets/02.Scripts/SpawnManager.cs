using UnityEngine;
using UnityEngine.Serialization;

public class SpawnManager : MonoBehaviour
{
    private float _minTime = 1f;
    private float _maxTime = 5f;

    private float _createTime;
    private float _currentTime;

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject monsterFactory;

    private void Start()
    {
        _createTime = Random.Range(_minTime, _maxTime);
    }
    
    private void Update()
    {
        _currentTime += Time.deltaTime;
        
        if (_createTime < _currentTime)
        {
            SpawnDrone();
        }
    }

    private void SpawnDrone()
    {
        _currentTime = 0;
        _createTime = Random.Range(_minTime, _maxTime);

        int randomIndex = Random.Range(0, spawnPoints.Length);

        GameObject drone = Instantiate(monsterFactory, transform);
        drone.transform.position = spawnPoints[randomIndex].position;
        drone.SetActive(true);
    }
}
