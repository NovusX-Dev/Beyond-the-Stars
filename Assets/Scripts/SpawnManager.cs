using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _enemiesContainer;
    [SerializeField] float _enemySpawnTime = 5f;

    [Header("PowerUps")]
    [SerializeField] GameObject[] _powerUps;
    [SerializeField] float _minPTime = 7f, _maxPTime = 15f;

    bool _canSpawn = true;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }


    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (_canSpawn)
        {
            var posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            var enemy = Instantiate(_enemyPrefab, posToSpawn , Quaternion.identity);
            enemy.transform.parent = _enemiesContainer.transform;

            yield return new WaitForSeconds(_enemySpawnTime);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (_canSpawn)
        {
            yield return new WaitForSeconds(Random.Range(_minPTime, _maxPTime));

            var posToSpawn = new Vector3(Random.Range(-6f, 7f), 7f, 0);
            int randomPowerup = Random.Range(0, 3);
            Instantiate(_powerUps[randomPowerup], posToSpawn, Quaternion.identity);
        }
    }

    public void OnPlayerDeath()
    {
        _canSpawn = false;
    }
}
